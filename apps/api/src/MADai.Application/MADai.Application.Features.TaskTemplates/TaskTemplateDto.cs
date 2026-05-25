using System;
using MADai.Domain.Enums;

namespace MADai.Application.Features.TaskTemplates;

public sealed record TaskTemplateDto(Guid Id, string Name, string? Description, TaskCategory Category, TaskPriority DefaultPriority, string? PromptTemplate, string? DefaultInputJson, string? Queue, int DefaultTimeoutSeconds, int DefaultMaxRetries, bool IsPublic, DateTime CreatedDate);
