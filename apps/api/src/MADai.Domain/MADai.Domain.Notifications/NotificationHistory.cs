using System;
using MADai.Domain.Common;

namespace MADai.Domain.Notifications;

public class NotificationHistory : Entity
{
	public Guid? UserId { get; set; }

	public Guid? CompanyId { get; set; }

	public string TemplateCode { get; set; } = string.Empty;


	public string Channel { get; set; } = "Email";


	public DateTime SentAt { get; set; } = DateTime.UtcNow;


	public string Recipient { get; set; } = string.Empty;


	public string Status { get; set; } = "Sent";


	public string? ErrorMessage { get; set; }
}
