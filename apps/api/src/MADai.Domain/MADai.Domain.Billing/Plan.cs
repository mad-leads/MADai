using MADai.Domain.Common;

namespace MADai.Domain.Billing;

public class Plan : AuditableEntity
{
	public string Name { get; set; } = string.Empty;


	public string Code { get; set; } = string.Empty;


	public string? Description { get; set; }

	public decimal MonthlyPrice { get; set; }

	public decimal AnnualPrice { get; set; }

	public string Currency { get; set; } = "USD";


	public int IncludedTasks { get; set; }

	public int IncludedStorageGb { get; set; }

	public int IncludedWorkers { get; set; }

	public bool IsPublic { get; set; } = true;


	public bool IsActive { get; set; } = true;


	public string? FeaturesJson { get; set; }
}
