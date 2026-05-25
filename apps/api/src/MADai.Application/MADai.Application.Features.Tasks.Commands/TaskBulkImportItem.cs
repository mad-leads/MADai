using MADai.Domain.Enums;

namespace MADai.Application.Features.Tasks.Commands;

public sealed record TaskBulkImportItem(string Title, string? Description, TaskCategory Category, TaskPriority? Priority, string? Queue, string? PromptPayload);
