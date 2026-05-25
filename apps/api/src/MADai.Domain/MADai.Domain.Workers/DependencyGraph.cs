using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class DependencyGraph : AuditableEntity
{
	public string RepositoryKey { get; set; } = string.Empty;


	public string GraphJson { get; set; } = "{}";


	public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

}
