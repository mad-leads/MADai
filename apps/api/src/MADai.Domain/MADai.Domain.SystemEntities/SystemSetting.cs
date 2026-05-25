using MADai.Domain.Common;

namespace MADai.Domain.SystemEntities;

public class SystemSetting : AuditableEntity
{
	public string Key { get; set; } = string.Empty;


	public string Category { get; set; } = "General";


	public string? Value { get; set; }

	public string? DataType { get; set; } = "string";


	public bool IsSecret { get; set; }

	public string? Description { get; set; }
}
