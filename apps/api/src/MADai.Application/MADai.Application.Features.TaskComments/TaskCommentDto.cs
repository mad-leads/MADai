using System;

namespace MADai.Application.Features.TaskComments;

public sealed record TaskCommentDto(Guid Id, Guid TaskId, Guid? AuthorUserId, string? AuthorEmail, string Body, bool IsSystem, DateTime CreatedDate);
