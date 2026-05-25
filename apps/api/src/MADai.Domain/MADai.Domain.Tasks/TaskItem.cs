using System;
using System.Collections.Generic;
using MADai.Domain.Common;
using MADai.Domain.Enums;
using MADai.Domain.Workers;

namespace MADai.Domain.Tasks;

public class TaskItem : TenantEntity
{
	public string Title { get; set; } = string.Empty;


	public string? Description { get; set; }

	public TaskCategory Category { get; set; }

	public TaskPriority Priority { get; set; } = TaskPriority.Normal;


	public MADai.Domain.Enums.TaskStatus Status { get; set; }

	public TaskOrigin Origin { get; set; }

	public string? QueueName { get; set; }

	public Guid? ParentTaskId { get; set; }

	public TaskItem? ParentTask { get; set; }

	public ICollection<TaskItem> Subtasks { get; set; } = new List<TaskItem>();


	public string? PromptPayload { get; set; }

	public string? InputPayload { get; set; }

	public string? OutputSummary { get; set; }

	public string? ResultPayload { get; set; }

	public string? ErrorMessage { get; set; }

	public string? ErrorStack { get; set; }

	public int Progress { get; set; }

	public int RetryCount { get; set; }

	public int MaxRetries { get; set; } = 3;


	public int TimeoutSeconds { get; set; } = 3600;


	public DateTime? ScheduledAt { get; set; }

	public DateTime? ClaimedAt { get; set; }

	public DateTime? StartedAt { get; set; }

	public DateTime? CompletedAt { get; set; }

	public DateTime? CancelledAt { get; set; }

	public DateTime? NextRetryAt { get; set; }

	public TimeSpan? EstimatedDuration { get; set; }

	public TimeSpan? ActualDuration
	{
		get
		{
			if (!CompletedAt.HasValue || !StartedAt.HasValue)
			{
				return null;
			}
			return CompletedAt.Value - StartedAt.Value;
		}
	}

	public Guid? ClaimedByWorkerId { get; set; }

	public WorkerNode? ClaimedByWorker { get; set; }

	public string? ClaimToken { get; set; }

	public Guid? TemplateId { get; set; }

	public TaskTemplate? Template { get; set; }

	public string? Tags { get; set; }

	public string? WorkspacePath { get; set; }

	public bool IsRecurring { get; set; }

	public string? CronExpression { get; set; }

	public DateTime? LastRunAt { get; set; }

	public bool IsDeadLetter { get; set; }

	public string? ValidationReport { get; set; }

	public ICollection<TaskDependency> Dependencies { get; set; } = new List<TaskDependency>();


	public ICollection<TaskDependency> DependentTasks { get; set; } = new List<TaskDependency>();


	public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();


	public ICollection<TaskLog> Logs { get; set; } = new List<TaskLog>();


	public ICollection<TaskArtifact> Artifacts { get; set; } = new List<TaskArtifact>();


	public ICollection<TaskExecution> Executions { get; set; } = new List<TaskExecution>();


	public ICollection<TaskAssignment> Assignments { get; set; } = new List<TaskAssignment>();


	public ICollection<TaskTagLink> TagLinks { get; set; } = new List<TaskTagLink>();

}
