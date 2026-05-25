using System;

namespace MADai.Application.Features.PersistentWorkers;

public sealed record RepositoryIntelligenceDto(string RepositoryKey, string RepositoryPath, string Summary, string ArchitectureJson, string RouteMapJson, string EndpointMapJson, string EntityMapJson, string DependencyGraphJson, string BuildInstructions, string ValidationInstructions, DateTime ScannedAt, string CacheFingerprint);
