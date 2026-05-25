using System;
using MADai.Domain.Common;

namespace MADai.Domain.Identity;

public class LoginHistory : Entity
{
	public Guid UserId { get; set; }

	public ApplicationUser? User { get; set; }

	public DateTime LoginAt { get; set; } = DateTime.UtcNow;


	public string? IpAddress { get; set; }

	public string? UserAgent { get; set; }

	public string? DeviceFingerprint { get; set; }

	public bool Success { get; set; }

	public string? FailureReason { get; set; }

	public string? Country { get; set; }

	public string? City { get; set; }
}
