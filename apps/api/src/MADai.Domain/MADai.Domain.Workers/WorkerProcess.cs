using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class WorkerProcess : AuditableEntity
{
	public Guid? WorkerNodeId { get; set; }

	public string RepositoryKey { get; set; } = string.Empty;


	public string ProcessKey { get; set; } = string.Empty;


	public int? ProcessId { get; set; }

	public string SessionId { get; set; } = string.Empty;


	public string Status { get; set; } = "Starting";


	public DateTime StartedAt { get; set; } = DateTime.UtcNow;


	public DateTime? LastSeenAt { get; set; }
}
