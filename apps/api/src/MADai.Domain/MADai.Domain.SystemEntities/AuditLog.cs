using System;
using MADai.Domain.Common;

namespace MADai.Domain.SystemEntities;

public class AuditLog : Entity
{
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;


	public Guid? ActorUserId { get; set; }

	public Guid? CompanyId { get; set; }

	public string Action { get; set; } = string.Empty;


	public string EntityType { get; set; } = string.Empty;


	public string? EntityId { get; set; }

	public string? IpAddress { get; set; }

	public string? Detail { get; set; }

	public string Severity { get; set; } = "Info";

}
