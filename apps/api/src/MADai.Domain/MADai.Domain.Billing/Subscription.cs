using System;
using MADai.Domain.Common;
using MADai.Domain.Enums;

namespace MADai.Domain.Billing;

public class Subscription : TenantEntity
{
	public Guid PlanId { get; set; }

	public Plan? Plan { get; set; }

	public SubscriptionStatus Status { get; set; }

	public DateTime StartDate { get; set; } = DateTime.UtcNow;


	public DateTime? CurrentPeriodEnd { get; set; }

	public DateTime? CancelledAt { get; set; }

	public string BillingCycle { get; set; } = "Monthly";


	public string? ExternalReference { get; set; }
}
