namespace MADai.Application.Features.TaskComments;

public sealed record CreateTaskCommentRequest(string Body, bool IsSystem = false);
