using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using MADai.Domain.Workers;
using MADai.Shared.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MADai.Application.Features.Workers;

public class WorkerQueueService : IWorkerQueueService
{
	private readonly IDbContextAccess _db;

	private readonly IDateTimeProvider _clock;

	private readonly IEventPublisher _publisher;

	private readonly ILogger<WorkerQueueService> _logger;

	public WorkerQueueService(IDbContextAccess db, IDateTimeProvider clock, IEventPublisher publisher, ILogger<WorkerQueueService> logger)
	{
		_db = db;
		_clock = clock;
		_publisher = publisher;
		_logger = logger;
	}

	public async Task<IReadOnlyList<TaskClaimResponseItem>> ClaimAsync(Guid workerId, TaskClaimRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		WorkerNode worker = (await _db.WorkerNodes.FirstOrDefaultAsync((WorkerNode w) => w.Id == workerId, cancellationToken)) ?? throw new NotFoundException("Worker", workerId);
		if (!worker.IsActive)
		{
			return Array.Empty<TaskClaimResponseItem>();
		}
		DateTime now = _clock.UtcNow;
		int maxClaim = Math.Clamp(request.MaxTasks, 1, Math.Max(1, worker.MaxConcurrency - worker.CurrentConcurrency));
		if (maxClaim == 0)
		{
			return Array.Empty<TaskClaimResponseItem>();
		}
		IReadOnlyList<string> queues = request.Queues;
		IReadOnlyList<string> readOnlyList2;
		if (queues == null || queues.Count <= 0)
		{
			IReadOnlyList<string> readOnlyList = new string[1] { worker.QueueName ?? "default" };
			readOnlyList2 = readOnlyList;
		}
		else
		{
			readOnlyList2 = request.Queues;
		}
		IReadOnlyList<string> queueNames = readOnlyList2;
		List<TaskItem> candidates = await (from t in _db.Tasks
			where ((int)t.Status == 10 || ((int)t.Status == 15 && t.ScheduledAt != null && t.ScheduledAt <= now)) && (t.NextRetryAt == null || t.NextRetryAt <= now) && !t.IsDeadLetter && (worker.CompanyId == null || t.CompanyId == worker.CompanyId) && (t.QueueName == null || queueNames.Contains<string>(t.QueueName))
			orderby t.Priority descending, t.CreatedDate
			select t).Take(maxClaim * 4).ToListAsync(cancellationToken);
		List<TaskClaimResponseItem> claimed = new List<TaskClaimResponseItem>();
		foreach (TaskItem task in candidates)
		{
			if (claimed.Count < maxClaim)
			{
				if (await AreDependenciesSatisfiedAsync(task.Id, cancellationToken))
				{
					task.Status = MADai.Domain.Enums.TaskStatus.Claimed;
					task.ClaimedAt = now;
					task.ClaimedByWorkerId = worker.Id;
					task.ClaimToken = Guid.NewGuid().ToString("N");
					worker.CurrentConcurrency = Math.Min(worker.MaxConcurrency, worker.CurrentConcurrency + 1);
					_db.TaskExecutions.Add(new TaskExecution
					{
						TaskId = task.Id,
						WorkerNodeId = worker.Id,
						AttemptNumber = task.RetryCount + 1,
						StartedAt = now,
						FinalStatus = MADai.Domain.Enums.TaskStatus.Running
					});
					claimed.Add(new TaskClaimResponseItem(task.Id, task.Title, task.Category, task.Priority, task.PromptPayload, task.InputPayload, task.TimeoutSeconds, task.RetryCount + 1, task.ClaimToken, task.WorkspacePath));
				}
				continue;
			}
			break;
		}
		if (claimed.Count > 0)
		{
			await _db.SaveChangesAsync(cancellationToken);
			foreach (TaskClaimResponseItem item in claimed)
			{
				await _publisher.PublishTaskUpdatedAsync(worker.CompanyId ?? Guid.Empty, item.TaskId, new
				{
					TaskId = item.TaskId,
					Status = MADai.Domain.Enums.TaskStatus.Claimed,
					WorkerId = worker.Id,
					Event = "Claimed"
				}, cancellationToken);
			}
			await _publisher.PublishWorkerUpdatedAsync(worker.CompanyId, worker.Id, new
			{
				Id = worker.Id,
				CurrentConcurrency = worker.CurrentConcurrency,
				Event = "Claimed"
			}, cancellationToken);
		}
		return claimed;
	}

	public async Task ReportProgressAsync(Guid workerId, Guid taskId, string claimToken, TaskProgressReport report, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaskItem task = await LoadClaimedTaskAsync(workerId, taskId, claimToken, cancellationToken);
		task.Progress = Math.Clamp(report.Progress, 0, 100);
		if (task.Status == MADai.Domain.Enums.TaskStatus.Claimed)
		{
			task.Status = MADai.Domain.Enums.TaskStatus.Running;
			TaskItem taskItem = task;
			DateTime? startedAt = taskItem.StartedAt;
			startedAt.GetValueOrDefault();
			if (!startedAt.HasValue)
			{
				DateTime utcNow = _clock.UtcNow;
				taskItem.StartedAt = utcNow;
			}
		}
		if (!string.IsNullOrWhiteSpace(report.CheckpointJson))
		{
			TaskExecution execution = await (from e in _db.TaskExecutions
				where e.TaskId == taskId && e.WorkerNodeId == workerId && e.CompletedAt == null
				orderby e.StartedAt descending
				select e).FirstOrDefaultAsync(cancellationToken);
			if (execution != null)
			{
				execution.CheckpointJson = report.CheckpointJson;
			}
		}
		await _db.SaveChangesAsync(cancellationToken);
		await _publisher.PublishTaskUpdatedAsync(task.CompanyId, task.Id, new
		{
			Id = task.Id,
			Status = task.Status,
			Progress = task.Progress,
			StatusMessage = report.StatusMessage,
			Event = "Progress"
		}, cancellationToken);
	}

	public async Task ReportLogAsync(Guid workerId, Guid taskId, string claimToken, TaskLogEntry entry, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaskItem task = await LoadClaimedTaskAsync(workerId, taskId, claimToken, cancellationToken);
		_db.TaskLogs.Add(new TaskLog
		{
			TaskId = task.Id,
			Timestamp = ((entry.Timestamp == default(DateTime)) ? _clock.UtcNow : entry.Timestamp),
			Level = entry.Level,
			Message = ((entry.Message.Length > 4000) ? entry.Message.Substring(0, 4000) : entry.Message),
			Source = entry.Source
		});
		await _db.SaveChangesAsync(cancellationToken);
	}

	public async Task CompleteAsync(Guid workerId, Guid taskId, string claimToken, TaskCompletionReport report, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaskItem task = await LoadClaimedTaskAsync(workerId, taskId, claimToken, cancellationToken);
		DateTime now = _clock.UtcNow;
		task.Status = (report.ValidationPassed ? MADai.Domain.Enums.TaskStatus.Completed : MADai.Domain.Enums.TaskStatus.AwaitingValidation);
		task.CompletedAt = now;
		task.Progress = 100;
		task.OutputSummary = report.OutputSummary;
		task.ResultPayload = report.ResultPayload;
		task.ValidationReport = report.ValidationReport;
		task.ErrorMessage = null;
		TaskExecution execution = await (from e in _db.TaskExecutions
			where e.TaskId == taskId && e.WorkerNodeId == workerId && e.CompletedAt == null
			orderby e.StartedAt descending
			select e).FirstOrDefaultAsync(cancellationToken);
		if (execution != null)
		{
			execution.CompletedAt = now;
			execution.FinalStatus = task.Status;
			execution.OutputSummary = report.OutputSummary;
		}
		WorkerNode worker = await _db.WorkerNodes.FirstOrDefaultAsync((WorkerNode w) => w.Id == workerId, cancellationToken);
		if (worker != null)
		{
			worker.CurrentConcurrency = Math.Max(0, worker.CurrentConcurrency - 1);
		}
		await _db.SaveChangesAsync(cancellationToken);
		await _publisher.PublishTaskUpdatedAsync(task.CompanyId, task.Id, new
		{
			Id = task.Id,
			Status = task.Status,
			Event = "Completed"
		}, cancellationToken);
		if (worker != null)
		{
			await _publisher.PublishWorkerUpdatedAsync(worker.CompanyId, worker.Id, new
			{
				Id = worker.Id,
				CurrentConcurrency = worker.CurrentConcurrency,
				Event = "Completed"
			}, cancellationToken);
		}
	}

	public async Task FailAsync(Guid workerId, Guid taskId, string claimToken, TaskFailureReport report, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaskItem task = await LoadClaimedTaskAsync(workerId, taskId, claimToken, cancellationToken);
		DateTime now = _clock.UtcNow;
		task.ErrorMessage = report.Reason;
		task.ErrorStack = report.StackTrace;
		_db.TaskFailures.Add(new TaskFailure
		{
			TaskId = task.Id,
			FailedAt = now,
			Reason = report.Reason,
			StackTrace = report.StackTrace,
			IsTransient = report.IsTransient,
			WorkerNodeId = workerId
		});
		TaskExecution execution = await (from e in _db.TaskExecutions
			where e.TaskId == taskId && e.WorkerNodeId == workerId && e.CompletedAt == null
			orderby e.StartedAt descending
			select e).FirstOrDefaultAsync(cancellationToken);
		if (execution != null)
		{
			execution.CompletedAt = now;
			execution.FinalStatus = MADai.Domain.Enums.TaskStatus.Failed;
			execution.ErrorMessage = report.Reason;
		}
		if (report.IsTransient && task.RetryCount < task.MaxRetries)
		{
			task.RetryCount++;
			TimeSpan delay = TimeSpan.FromSeconds(Math.Min(900.0, Math.Pow(2.0, task.RetryCount) * 15.0));
			task.NextRetryAt = now.Add(delay);
			task.Status = MADai.Domain.Enums.TaskStatus.Queued;
			task.ClaimedByWorkerId = null;
			task.ClaimToken = null;
			task.ClaimedAt = null;
		}
		else
		{
			task.Status = MADai.Domain.Enums.TaskStatus.Failed;
			if (task.RetryCount >= task.MaxRetries)
			{
				task.IsDeadLetter = true;
				task.Status = MADai.Domain.Enums.TaskStatus.DeadLetter;
			}
		}
		WorkerNode worker = await _db.WorkerNodes.FirstOrDefaultAsync((WorkerNode w) => w.Id == workerId, cancellationToken);
		if (worker != null)
		{
			worker.CurrentConcurrency = Math.Max(0, worker.CurrentConcurrency - 1);
		}
		await _db.SaveChangesAsync(cancellationToken);
		await _publisher.PublishTaskUpdatedAsync(task.CompanyId, task.Id, new
		{
			Id = task.Id,
			Status = task.Status,
			Event = "Failed",
			NextRetryAt = task.NextRetryAt,
			IsDeadLetter = task.IsDeadLetter
		}, cancellationToken);
		_logger.LogWarning("Task {TaskId} failed: {Reason}", task.Id, report.Reason);
	}

	public async Task HeartbeatAsync(Guid workerId, WorkerHeartbeatRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		WorkerNode worker = (await _db.WorkerNodes.FirstOrDefaultAsync((WorkerNode w) => w.Id == workerId, cancellationToken)) ?? throw new NotFoundException("Worker", workerId);
		DateTime now = _clock.UtcNow;
		worker.LastHeartbeatAt = now;
		worker.Status = request.Status;
		worker.CurrentConcurrency = request.ActiveTasks;
		_db.WorkerHeartbeats.Add(new WorkerHeartbeat
		{
			WorkerNodeId = worker.Id,
			Timestamp = now,
			Status = request.Status,
			ActiveTasks = request.ActiveTasks,
			CpuPercent = request.CpuPercent,
			MemoryMb = request.MemoryMb,
			DiskFreeGb = request.DiskFreeGb,
			Notes = request.Notes
		});
		await _db.SaveChangesAsync(cancellationToken);
		await _publisher.PublishWorkerUpdatedAsync(worker.CompanyId, worker.Id, new
		{
			Id = worker.Id,
			Status = worker.Status,
			CurrentConcurrency = worker.CurrentConcurrency,
			Event = "Heartbeat"
		}, cancellationToken);
	}

	private async Task<TaskItem> LoadClaimedTaskAsync(Guid workerId, Guid taskId, string claimToken, CancellationToken cancellationToken)
	{
		TaskItem task = (await _db.Tasks.FirstOrDefaultAsync((TaskItem t) => t.Id == taskId, cancellationToken)) ?? throw new NotFoundException("Task", taskId);
		if (task.ClaimedByWorkerId != workerId || task.ClaimToken != claimToken)
		{
			throw new ForbiddenException("Claim token mismatch.");
		}
		return task;
	}

	private async Task<bool> AreDependenciesSatisfiedAsync(Guid taskId, CancellationToken cancellationToken)
	{
		return (await (from d in _db.TaskDependencies
			where d.TaskId == taskId
			join t in _db.Tasks on d.DependsOnTaskId equals t.Id
			select new { d.IsHardDependency, t.Status }).ToListAsync(cancellationToken)).All(d => !d.IsHardDependency || d.Status == MADai.Domain.Enums.TaskStatus.Completed);
	}
}
