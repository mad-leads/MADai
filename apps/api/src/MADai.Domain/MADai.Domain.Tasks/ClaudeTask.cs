using MADai.Domain.Common;
using MADai.Domain.Enums;

namespace MADai.Domain.Tasks;

public class ClaudeTask : AuditableEntity
{
	public string Title { get; set; } = string.Empty;


	public string? Description { get; set; }

	public string? Notes { get; set; }

	public ClaudeTaskStatus Status { get; set; }

	public ClaudeTaskPriority Priority { get; set; } = ClaudeTaskPriority.Normal;


	public string? AttachmentsJson { get; set; }
}
