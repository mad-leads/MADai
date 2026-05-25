namespace MADai.Application.Features.PersistentWorkers;

public sealed record InjectTaskRequest(string RepositoryKey, string Title, string Prompt, string? WorkspacePath, int EstimatedTokens = 0, bool AllowSessionReuse = true);
