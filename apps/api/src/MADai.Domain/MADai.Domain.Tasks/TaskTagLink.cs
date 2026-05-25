using System;
using MADai.Domain.Common;

namespace MADai.Domain.Tasks;

public class TaskTagLink : Entity
{
	public Guid TaskId { get; set; }

	public TaskItem? Task { get; set; }

	public Guid TagId { get; set; }

	public TaskTag? Tag { get; set; }
}
