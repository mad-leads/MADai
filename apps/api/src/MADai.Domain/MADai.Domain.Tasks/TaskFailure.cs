using System;
using MADai.Domain.Common;

namespace MADai.Domain.Tasks;

public class TaskFailure : Entity
{
	public Guid TaskId { get; set; }

	public TaskItem? Task { get; set; }

	public DateTime FailedAt { get; set; } = DateTime.UtcNow;


	public string Reason { get; set; } = string.Empty;


	public string? StackTrace { get; set; }

	public string? Category { get; set; }

	public bool IsTransient { get; set; }

	public Guid? WorkerNodeId { get; set; }
}
