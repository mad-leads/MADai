using System;
using MADai.Domain.Common;
using MADai.Domain.Identity;

namespace MADai.Domain.Notifications;

public class Notification : Entity
{
	public Guid? UserId { get; set; }

	public ApplicationUser? User { get; set; }

	public Guid? CompanyId { get; set; }

	public string Channel { get; set; } = "InApp";


	public string Severity { get; set; } = "Info";


	public string Title { get; set; } = string.Empty;


	public string Body { get; set; } = string.Empty;


	public string? Url { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


	public DateTime? ReadAt { get; set; }

	public DateTime? DismissedAt { get; set; }

	public string? Tags { get; set; }
}
