using System;
using MADai.Domain.Common;
using MADai.Domain.Enums;

namespace MADai.Domain.Billing;

public class Invoice : TenantEntity
{
	public string Number { get; set; } = string.Empty;


	public InvoiceStatus Status { get; set; }

	public decimal Subtotal { get; set; }

	public decimal Tax { get; set; }

	public decimal Total { get; set; }

	public string Currency { get; set; } = "USD";


	public DateTime IssuedAt { get; set; } = DateTime.UtcNow;


	public DateTime? DueAt { get; set; }

	public DateTime? PaidAt { get; set; }

	public string? LineItemsJson { get; set; }

	public Guid? SubscriptionId { get; set; }

	public Subscription? Subscription { get; set; }
}
