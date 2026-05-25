using MADai.Domain.Common;

namespace MADai.Domain.Notifications;

public class NotificationTemplate : AuditableEntity
{
	public string Code { get; set; } = string.Empty;


	public string Name { get; set; } = string.Empty;


	public string Channel { get; set; } = "Email";


	public string Subject { get; set; } = string.Empty;


	public string Body { get; set; } = string.Empty;


	public bool IsActive { get; set; } = true;

}
