using System;
using MADai.Domain.Common;

namespace MADai.Domain.SystemEntities;

public class WebhookEndpoint : TenantEntity
{
	public string Url { get; set; } = string.Empty;


	public string Secret { get; set; } = string.Empty;


	public string? EventsCsv { get; set; }

	public bool IsActive { get; set; } = true;


	public DateTime? LastSuccessAt { get; set; }

	public DateTime? LastFailureAt { get; set; }

	public int ConsecutiveFailures { get; set; }
}
