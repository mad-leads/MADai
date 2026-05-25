using MADai.Domain.Common;

namespace MADai.Domain.Audit;

public class OptimizationSuggestion : AuditableEntity
{
	public string Area { get; set; } = string.Empty;


	public string Title { get; set; } = string.Empty;


	public string Description { get; set; } = string.Empty;


	public double EstimatedImpact { get; set; }

	public string Status { get; set; } = "Pending";

}
