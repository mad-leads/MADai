using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class RepositoryIntelligence : AuditableEntity
{
	public string RepositoryKey { get; set; } = string.Empty;


	public string RepositoryPath { get; set; } = string.Empty;


	public string? BranchName { get; set; }

	public string? CommitSha { get; set; }

	public string Summary { get; set; } = string.Empty;


	public string ArchitectureJson { get; set; } = "{}";


	public string RouteMapJson { get; set; } = "{}";


	public string EndpointMapJson { get; set; } = "{}";


	public string EntityMapJson { get; set; } = "{}";


	public string DependencyGraphJson { get; set; } = "{}";


	public string BuildInstructions { get; set; } = string.Empty;


	public string ValidationInstructions { get; set; } = string.Empty;


	public DateTime ScannedAt { get; set; } = DateTime.UtcNow;


	public string CacheFingerprint { get; set; } = string.Empty;

}
