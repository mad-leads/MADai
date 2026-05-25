using System;
using MADai.Domain.Common;
using MADai.Domain.Enums;

namespace MADai.Domain.Billing;

public class Payment : TenantEntity
{
	public Guid InvoiceId { get; set; }

	public Invoice? Invoice { get; set; }

	public decimal Amount { get; set; }

	public string Currency { get; set; } = "USD";


	public PaymentStatus Status { get; set; }

	public string Provider { get; set; } = "Manual";


	public string? ProviderReference { get; set; }

	public DateTime? ProcessedAt { get; set; }
}
