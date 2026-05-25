using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class RepositoryCache : AuditableEntity
{
	public string RepositoryKey { get; set; } = string.Empty;


	public string CacheKey { get; set; } = string.Empty;


	public string CacheJson { get; set; } = "{}";


	public DateTime ExpiresAt { get; set; }
}
