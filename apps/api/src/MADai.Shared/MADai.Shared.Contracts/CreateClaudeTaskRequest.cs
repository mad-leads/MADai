using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record CreateClaudeTaskRequest(string Title, string? Description, string? Notes, ClaudeTaskPriority? Priority);
