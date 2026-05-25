using System;
using MADai.Domain.Common;
using MADai.Domain.Enums;
using MADai.Domain.Workers;

namespace MADai.Domain.Tasks;

public class TaskExecution : Entity
{
	public Guid TaskId { get; set; }

	public TaskItem? Task { get; set; }

	public Guid WorkerNodeId { get; set; }

	public WorkerNode? WorkerNode { get; set; }

	public int AttemptNumber { get; set; }

	public DateTime StartedAt { get; set; } = DateTime.UtcNow;


	public DateTime? CompletedAt { get; set; }

	public MADai.Domain.Enums.TaskStatus FinalStatus { get; set; }

	public string? CheckpointJson { get; set; }

	public string? WorkspacePath { get; set; }

	public string? OutputSummary { get; set; }

	public string? ErrorMessage { get; set; }

	public TimeSpan? Duration
	{
		get
		{
			if (!CompletedAt.HasValue)
			{
				return null;
			}
			return CompletedAt.Value - StartedAt;
		}
	}
}
