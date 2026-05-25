using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class SessionCheckpoint : AuditableEntity
{
	public Guid? WorkerNodeId { get; set; }

	public string RepositoryKey { get; set; } = string.Empty;


	public string SessionId { get; set; } = string.Empty;


	public string Summary { get; set; } = string.Empty;


	public string CheckpointJson { get; set; } = "{}";


	public DateTime CapturedAt { get; set; } = DateTime.UtcNow;

}
