using System;
using MADai.Domain.Common;

namespace MADai.Domain.Webhooks;

public class WebhookEvent : Entity
{
	public Guid CompanyId { get; set; }

	public Guid EndpointId { get; set; }

	public string EventType { get; set; } = string.Empty;


	public string PayloadJson { get; set; } = string.Empty;


	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


	public DateTime? DeliveredAt { get; set; }

	public DateTime? NextAttemptAt { get; set; }

	public int AttemptCount { get; set; }

	public int? LastResponseStatus { get; set; }

	public string? LastError { get; set; }

	public string Status { get; set; } = "Pending";

}
