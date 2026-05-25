using MADai.Domain.Common;
using MADai.Domain.Enums;

namespace MADai.Domain.Tasks;

/// <summary>
/// Developer self-improvement task queue. Distinct from <see cref="TaskItem"/> (end-user AI
/// task queue): a Claude Code worker claims these and writes code against this repo.
/// </summary>
public class ClaudeTask : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public ClaudeTaskStatus Status { get; set; } = ClaudeTaskStatus.Pending;
    public ClaudeTaskPriority Priority { get; set; } = ClaudeTaskPriority.Normal;
    /// <summary>
    /// JSON array of <c>{fileName, storagePath, contentType, sizeBytes}</c>. Stored inline
    /// rather than as a separate table because attachments are small and never queried.
    /// </summary>
    public string? AttachmentsJson { get; set; }
}

public class ClaudePromptTemplate : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Content { get; set; } = string.Empty;
}
