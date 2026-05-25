using MADai.Domain.Common;

namespace MADai.Domain.Identity;

public class Permission : Entity
{
	public string Code { get; set; } = string.Empty;


	public string DisplayName { get; set; } = string.Empty;


	public string? Description { get; set; }

	public string Category { get; set; } = "General";

}
