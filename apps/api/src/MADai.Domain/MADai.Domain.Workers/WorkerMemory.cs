using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class WorkerMemory : AuditableEntity
{
	public Guid? WorkerNodeId { get; set; }

	public string RepositoryKey { get; set; } = string.Empty;


	public string MemoryKey { get; set; } = string.Empty;


	public string MemoryJson { get; set; } = "{}";


	public DateTime LastUsedAt { get; set; } = DateTime.UtcNow;

}
