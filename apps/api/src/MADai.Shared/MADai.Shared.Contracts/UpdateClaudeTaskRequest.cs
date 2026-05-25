using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record UpdateClaudeTaskRequest(string? Title, string? Description, string? Notes, ClaudeTaskStatus? Status, ClaudeTaskPriority? Priority);
