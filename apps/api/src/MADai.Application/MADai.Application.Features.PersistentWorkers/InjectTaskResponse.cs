namespace MADai.Application.Features.PersistentWorkers;

public sealed record InjectTaskResponse(string SessionId, string RepositoryKey, bool ReusedSession, bool RotationRecommended, string WorkerProcessKey, string InjectionPath);
