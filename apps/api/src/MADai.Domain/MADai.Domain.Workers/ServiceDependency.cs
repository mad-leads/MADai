using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class ServiceDependency : AuditableEntity
{
	public string ServiceName { get; set; } = string.Empty;


	public string DependsOnServiceName { get; set; } = string.Empty;


	public bool IsRequired { get; set; } = true;

}
