using System;
using MADai.Domain.Common;

namespace MADai.Domain.Tasks;

public class TaskDependency : Entity
{
	public Guid TaskId { get; set; }

	public TaskItem? Task { get; set; }

	public Guid DependsOnTaskId { get; set; }

	public TaskItem? DependsOnTask { get; set; }

	public string DependencyType { get; set; } = "FinishToStart";


	public bool IsHardDependency { get; set; } = true;

}
