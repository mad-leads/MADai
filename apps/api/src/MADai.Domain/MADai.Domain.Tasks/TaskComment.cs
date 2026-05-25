using System;
using MADai.Domain.Common;

namespace MADai.Domain.Tasks;

public class TaskComment : AuditableEntity
{
	public Guid TaskId { get; set; }

	public TaskItem? Task { get; set; }

	public Guid? AuthorUserId { get; set; }

	public string Body { get; set; } = string.Empty;


	public bool IsSystem { get; set; }
}
