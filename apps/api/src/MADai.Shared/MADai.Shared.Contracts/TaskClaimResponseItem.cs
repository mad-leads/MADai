using System;
using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record TaskClaimResponseItem(Guid TaskId, string Title, TaskCategory Category, TaskPriority Priority, string? PromptPayload, string? InputPayload, int TimeoutSeconds, int AttemptNumber, string ClaimToken, string? WorkspacePath);
