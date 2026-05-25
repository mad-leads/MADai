using System;
using MADai.Domain.Common;
using MADai.Domain.Identity;

namespace MADai.Domain.Notifications;

public class NotificationPreference : Entity
{
	public Guid UserId { get; set; }

	public ApplicationUser? User { get; set; }

	public string Category { get; set; } = string.Empty;


	public bool Email { get; set; } = true;


	public bool InApp { get; set; } = true;


	public bool Push { get; set; }

	public bool Webhook { get; set; }
}
