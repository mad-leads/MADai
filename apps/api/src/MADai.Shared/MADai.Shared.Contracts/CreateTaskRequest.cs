using System;
using System.Collections.Generic;
using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record CreateTaskRequest(string Title, string? Description, TaskCategory Category, TaskPriority Priority, string? Queue, string? PromptPayload, string? InputPayload, int? TimeoutSeconds, int? MaxRetries, DateTime? ScheduledAt, IReadOnlyList<Guid>? DependsOnTaskIds, IReadOnlyList<string>? Tags, Guid? TemplateId, Guid? ParentTaskId, bool IsRecurring, string? CronExpression);
