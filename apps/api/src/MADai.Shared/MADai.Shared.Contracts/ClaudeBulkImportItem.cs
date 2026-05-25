using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record ClaudeBulkImportItem(string Title, string? Description, string? Notes, ClaudeTaskPriority? Priority);
