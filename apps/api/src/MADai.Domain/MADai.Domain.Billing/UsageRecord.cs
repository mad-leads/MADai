using System;
using MADai.Domain.Common;

namespace MADai.Domain.Billing;

public class UsageRecord : TenantEntity
{
	public DateTime PeriodStart { get; set; }

	public DateTime PeriodEnd { get; set; }

	public string MetricName { get; set; } = string.Empty;


	public double Quantity { get; set; }

	public string? Notes { get; set; }
}
