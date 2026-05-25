using MADai.Domain.Enums;

namespace MADai.Application.Features.TaskTemplates;

public sealed record UpdateTaskTemplateRequest(string? Name, string? Description, TaskCategory? Category, TaskPriority? DefaultPriority, string? PromptTemplate, string? DefaultInputJson, string? Queue, int? DefaultTimeoutSeconds, int? DefaultMaxRetries, bool? IsPublic);
