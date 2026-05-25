using System;
using System.Collections.Generic;
using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record TaskDetailDto(Guid Id, string Title, string? Description, TaskCategory Category, TaskPriority Priority, MADai.Domain.Enums.TaskStatus Status, TaskOrigin Origin, int Progress, int RetryCount, int MaxRetries, int TimeoutSeconds, DateTime CreatedDate, DateTime? ScheduledAt, DateTime? ClaimedAt, DateTime? StartedAt, DateTime? CompletedAt, DateTime? CancelledAt, DateTime? NextRetryAt, Guid? ClaimedByWorkerId, string? ClaimedByWorkerName, string? PromptPayload, string? InputPayload, string? OutputSummary, string? ResultPayload, string? ErrorMessage, string? ValidationReport, string? WorkspacePath, Guid? ParentTaskId, bool IsRecurring, string? CronExpression, IReadOnlyList<string> Tags, IReadOnlyList<TaskDependencyDto> Dependencies, IReadOnlyList<TaskArtifactDto> Artifacts, Guid CompanyId, string? Queue);
