using System;
using MADai.Domain.Common;

namespace MADai.Domain.Identity;

public class RefreshToken : Entity
{
	public Guid UserId { get; set; }

	public ApplicationUser? User { get; set; }

	public string Token { get; set; } = string.Empty;


	public DateTime ExpiresAt { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


	public string? CreatedByIp { get; set; }

	public DateTime? RevokedAt { get; set; }

	public string? RevokedByIp { get; set; }

	public string? ReplacedByToken { get; set; }

	public string? ReasonRevoked { get; set; }

	public bool IsActive
	{
		get
		{
			if (!RevokedAt.HasValue)
			{
				return DateTime.UtcNow < ExpiresAt;
			}
			return false;
		}
	}
}
