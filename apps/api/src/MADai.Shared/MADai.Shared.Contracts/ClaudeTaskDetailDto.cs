using System;
using System.Collections.Generic;
using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record ClaudeTaskDetailDto(Guid Id, string Title, string? Description, string? Notes, ClaudeTaskStatus Status, ClaudeTaskPriority Priority, IReadOnlyList<ClaudeTaskAttachmentDto> Attachments, DateTime CreatedDate, DateTime? ModifiedDate);
