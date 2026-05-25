using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.PersistentWorkers;

public class SessionOrchestrator : ISessionOrchestrator
{
	private const int TokenRotationThreshold = 160000;

	private readonly IDbContextAccess _db;

	private readonly IEventPublisher _publisher;

	public SessionOrchestrator(IDbContextAccess db, IEventPublisher publisher)
	{
		_db = db;
		_publisher = publisher;
	}

	public async Task<InjectTaskResponse> InjectAsync(InjectTaskRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		InjectTaskRequest request2 = request;
		RepositoryIntelligence intelligence = await _db.RepositoryIntelligence.FirstOrDefaultAsync((RepositoryIntelligence x) => x.RepositoryKey == request2.RepositoryKey, cancellationToken);
		if (intelligence == null)
		{
			throw new InvalidOperationException("Repository intelligence is missing for " + request2.RepositoryKey + ".");
		}
		WorkerProcess process = await (from x in _db.WorkerProcesses
			where x.RepositoryKey == request2.RepositoryKey && x.Status != "Stopped"
			orderby x.LastSeenAt ?? x.StartedAt descending
			select x).FirstOrDefaultAsync(cancellationToken);
		bool reused = request2.AllowSessionReuse && process != null;
		if (process == null)
		{
			process = new WorkerProcess
			{
				RepositoryKey = request2.RepositoryKey,
				ProcessKey = "repo-worker:" + request2.RepositoryKey,
				SessionId = $"session-{request2.RepositoryKey}-{DateTime.UtcNow:yyyyMMddHHmmss}",
				Status = "Idle",
				StartedAt = DateTime.UtcNow,
				LastSeenAt = DateTime.UtcNow
			};
			_db.WorkerProcesses.Add(process);
		}
		process.LastSeenAt = DateTime.UtcNow;
		process.Status = "Busy";
		int tokens = ((request2.EstimatedTokens > 0) ? request2.EstimatedTokens : Math.Clamp(request2.Prompt.Length / 4, 200, 8000));
		_db.TokenUsage.Add(new TokenUsage
		{
			RepositoryKey = request2.RepositoryKey,
			SessionId = process.SessionId,
			PromptTokens = tokens,
			CompletionTokens = 0,
			TotalTokens = tokens
		});
		int sessionTotal = (await _db.TokenUsage.Where((TokenUsage x) => x.RepositoryKey == request2.RepositoryKey && x.SessionId == process.SessionId).SumAsync((Expression<Func<TokenUsage, int?>>)((TokenUsage x) => x.TotalTokens), cancellationToken)).GetValueOrDefault();
		bool rotationRecommended = sessionTotal + tokens > 160000;
		var injection = new
		{
			Title = request2.Title,
			Prompt = request2.Prompt,
			WorkspacePath = request2.WorkspacePath,
			IntelligenceSummary = intelligence.Summary,
			InjectedAt = DateTime.UtcNow
		};
		_db.WorkerMemory.Add(new WorkerMemory
		{
			WorkerNodeId = process.WorkerNodeId,
			RepositoryKey = request2.RepositoryKey,
			MemoryKey = $"task-injection:{DateTime.UtcNow:yyyyMMddHHmmssfff}",
			MemoryJson = JsonSerializer.Serialize(injection),
			LastUsedAt = DateTime.UtcNow
		});
		_db.SessionMetrics.Add(new SessionMetric
		{
			WorkerNodeId = process.WorkerNodeId,
			RepositoryKey = request2.RepositoryKey,
			SessionId = process.SessionId,
			ActiveTaskCount = 1,
			EstimatedTokens = sessionTotal + tokens,
			MemoryMb = 0.0,
			Health = (rotationRecommended ? "RotationRecommended" : "Healthy"),
			Notes = "Task injected into persistent repository worker session."
		});
		await _db.SaveChangesAsync(cancellationToken);
		await _publisher.PublishWorkerUpdatedAsync(null, process.WorkerNodeId ?? Guid.Empty, new
		{
			Event = "TaskInjected",
			RepositoryKey = request2.RepositoryKey,
			SessionId = process.SessionId,
			rotationRecommended = rotationRecommended
		}, cancellationToken);
		return new InjectTaskResponse(process.SessionId, request2.RepositoryKey, reused, rotationRecommended, process.ProcessKey, "WorkerMemory/" + request2.RepositoryKey + "/" + process.SessionId);
	}

	public async Task RotateAsync(SessionRotationRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		SessionRotationRequest request2 = request;
		WorkerProcess process = await _db.WorkerProcesses.FirstOrDefaultAsync((WorkerProcess x) => x.RepositoryKey == request2.RepositoryKey && x.SessionId == request2.SessionId, cancellationToken);
		string newSessionId = $"session-{request2.RepositoryKey}-{DateTime.UtcNow:yyyyMMddHHmmss}";
		_db.SessionCheckpoints.Add(new SessionCheckpoint
		{
			WorkerNodeId = process?.WorkerNodeId,
			RepositoryKey = request2.RepositoryKey,
			SessionId = request2.SessionId,
			Summary = (request2.Summary ?? "Session rotated without supplied summary."),
			CapturedAt = DateTime.UtcNow
		});
		_db.SessionRotations.Add(new SessionRotation
		{
			WorkerNodeId = process?.WorkerNodeId,
			RepositoryKey = request2.RepositoryKey,
			OldSessionId = request2.SessionId,
			NewSessionId = newSessionId,
			Reason = request2.Reason,
			SummaryBeforeRotation = (request2.Summary ?? string.Empty),
			RotatedAt = DateTime.UtcNow
		});
		if (process != null)
		{
			process.SessionId = newSessionId;
			process.Status = "Idle";
			process.LastSeenAt = DateTime.UtcNow;
		}
		await _db.SaveChangesAsync(cancellationToken);
		await _publisher.PublishWorkerUpdatedAsync(null, process?.WorkerNodeId ?? Guid.Empty, new
		{
			Event = "SessionRotated",
			RepositoryKey = request2.RepositoryKey,
			OldSessionId = request2.SessionId,
			NewSessionId = newSessionId
		}, cancellationToken);
	}
}
