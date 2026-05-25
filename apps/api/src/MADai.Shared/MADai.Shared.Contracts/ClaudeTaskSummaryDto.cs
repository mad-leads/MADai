using System;
using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record ClaudeTaskSummaryDto(Guid Id, string Title, ClaudeTaskStatus Status, ClaudeTaskPriority Priority, int AttachmentCount, DateTime CreatedDate, DateTime? ModifiedDate);
