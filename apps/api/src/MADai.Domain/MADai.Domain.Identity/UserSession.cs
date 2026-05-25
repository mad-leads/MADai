using System;
using MADai.Domain.Common;

namespace MADai.Domain.Identity;

public class UserSession : Entity
{
	public Guid UserId { get; set; }

	public ApplicationUser? User { get; set; }

	public string SessionToken { get; set; } = string.Empty;


	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


	public DateTime? LastSeenAt { get; set; }

	public DateTime? ExpiresAt { get; set; }

	public string? IpAddress { get; set; }

	public string? UserAgent { get; set; }

	public string? DeviceFingerprint { get; set; }

	public bool IsActive { get; set; } = true;

}
