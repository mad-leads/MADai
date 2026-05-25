using MADai.Domain.Common;
using MADai.Domain.Enums;

namespace MADai.Domain.Tasks;

public class TaskTemplate : TenantEntity
{
	public string Name { get; set; } = string.Empty;


	public string? Description { get; set; }

	public TaskCategory Category { get; set; }

	public TaskPriority DefaultPriority { get; set; } = TaskPriority.Normal;


	public string? PromptTemplate { get; set; }

	public string? DefaultInputJson { get; set; }

	public string? QueueName { get; set; }

	public int DefaultTimeoutSeconds { get; set; } = 3600;


	public int DefaultMaxRetries { get; set; } = 3;


	public bool IsPublic { get; set; }

	public string? RequiredCapabilities { get; set; }
}
