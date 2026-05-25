using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class NativeService : AuditableEntity
{
	public string Name { get; set; } = string.Empty;


	public string Kind { get; set; } = "WindowsProcess";


	public string Command { get; set; } = string.Empty;


	public string WorkingDirectory { get; set; } = string.Empty;


	public int StartupOrder { get; set; }

	public bool IsEnabled { get; set; } = true;


	public string? HealthCheckCommand { get; set; }
}
