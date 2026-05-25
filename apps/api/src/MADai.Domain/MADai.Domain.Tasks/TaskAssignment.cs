using System;
using MADai.Domain.Common;
using MADai.Domain.Workers;

namespace MADai.Domain.Tasks;

public class TaskAssignment : Entity
{
	public Guid TaskId { get; set; }

	public TaskItem? Task { get; set; }

	public Guid? AssignedUserId { get; set; }

	public Guid? AssignedWorkerId { get; set; }

	public WorkerNode? AssignedWorker { get; set; }

	public DateTime AssignedAt { get; set; } = DateTime.UtcNow;


	public string AssignmentType { get; set; } = "Auto";


	public string? Notes { get; set; }
}
