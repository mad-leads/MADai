using System;
using MADai.Domain.Common;

namespace MADai.Domain.Billing;

public class CreditLedger : TenantEntity
{
	public decimal Amount { get; set; }

	public string Currency { get; set; } = "USD";


	public string Reason { get; set; } = string.Empty;


	public string? Reference { get; set; }

	public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

}
