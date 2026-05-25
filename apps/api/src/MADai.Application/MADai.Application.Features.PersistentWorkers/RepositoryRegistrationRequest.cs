namespace MADai.Application.Features.PersistentWorkers;

public sealed record RepositoryRegistrationRequest(string RepositoryKey, string RepositoryPath, string? BranchName);
