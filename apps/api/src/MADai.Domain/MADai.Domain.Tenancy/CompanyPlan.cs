using MADai.Domain.Common;

namespace MADai.Domain.Tenancy;

public class CompanyPlan : AuditableEntity
{
	public string Name { get; set; } = string.Empty;


	public string Code { get; set; } = string.Empty;


	public decimal MonthlyPrice { get; set; }

	public decimal AnnualPrice { get; set; }

	public int MaxConcurrentTasks { get; set; }

	public int MaxStorageGb { get; set; }

	public int MaxWorkers { get; set; }

	public int MaxUsers { get; set; }

	public bool IncludesSelfHealing { get; set; }

	public bool IncludesPrioritySupport { get; set; }

	public string? Features { get; set; }

	public bool IsActive { get; set; } = true;

}
