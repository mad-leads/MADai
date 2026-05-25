using MADai.Domain.Common;

namespace MADai.Domain.SystemEntities;

public class FeatureFlag : AuditableEntity
{
	public string Key { get; set; } = string.Empty;


	public string Name { get; set; } = string.Empty;


	public string? Description { get; set; }

	public bool IsEnabled { get; set; }

	public string? Audience { get; set; }

	public string? Configuration { get; set; }
}
