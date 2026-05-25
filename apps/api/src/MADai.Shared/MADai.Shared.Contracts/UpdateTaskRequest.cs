using System;
using System.Collections.Generic;
using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record UpdateTaskRequest(string? Title, string? Description, TaskPriority? Priority, string? Queue, DateTime? ScheduledAt, int? MaxRetries, int? TimeoutSeconds, IReadOnlyList<string>? Tags);
