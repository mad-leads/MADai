using System;
using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record TaskSummaryDto(Guid Id, string Title, TaskCategory Category, TaskPriority Priority, MADai.Domain.Enums.TaskStatus Status, int Progress, DateTime CreatedDate, DateTime? ScheduledAt, DateTime? StartedAt, DateTime? CompletedAt, int RetryCount, int MaxRetries, Guid? ClaimedByWorkerId, string? ClaimedByWorkerName, Guid CompanyId, string? Queue);
