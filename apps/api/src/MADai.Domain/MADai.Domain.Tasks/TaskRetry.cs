using System;
using MADai.Domain.Common;

namespace MADai.Domain.Tasks;

public class TaskRetry : Entity
{
	public Guid TaskId { get; set; }

	public TaskItem? Task { get; set; }

	public int AttemptNumber { get; set; }

	public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;


	public string? FailureReason { get; set; }

	public string? Strategy { get; set; }

	public Guid? WorkerNodeId { get; set; }
}
