using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class ArchitectureSummary : AuditableEntity
{
	public string RepositoryKey { get; set; } = string.Empty;


	public string Scope { get; set; } = "repository";


	public string Content { get; set; } = string.Empty;


	public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

}
