using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class SessionRotation : AuditableEntity
{
	public Guid? WorkerNodeId { get; set; }

	public string RepositoryKey { get; set; } = string.Empty;


	public string OldSessionId { get; set; } = string.Empty;


	public string? NewSessionId { get; set; }

	public string Reason { get; set; } = string.Empty;


	public string SummaryBeforeRotation { get; set; } = string.Empty;


	public DateTime RotatedAt { get; set; } = DateTime.UtcNow;

}
