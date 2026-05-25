namespace MADai.Application.Features.PersistentWorkers;

public sealed record SessionRotationRequest(string RepositoryKey, string SessionId, string Reason, string? Summary);
