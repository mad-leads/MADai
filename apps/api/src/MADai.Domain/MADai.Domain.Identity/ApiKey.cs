using System;
using MADai.Domain.Common;

namespace MADai.Domain.Identity;

public class ApiKey : AuditableEntity
{
	public Guid UserId { get; set; }

	public ApplicationUser? User { get; set; }

	public Guid? CompanyId { get; set; }

	public string Name { get; set; } = string.Empty;


	public string KeyPrefix { get; set; } = string.Empty;


	public string KeyHash { get; set; } = string.Empty;


	public DateTime? ExpiresAt { get; set; }

	public DateTime? LastUsedAt { get; set; }

	public string? Scopes { get; set; }

	public bool IsRevoked { get; set; }

	public DateTime? RevokedAt { get; set; }
}
