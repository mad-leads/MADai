using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class RouteMap : AuditableEntity
{
	public string RepositoryKey { get; set; } = string.Empty;


	public string Kind { get; set; } = "Angular";


	public string MapJson { get; set; } = "{}";


	public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

}
