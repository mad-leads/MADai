using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class WorkerQueue : AuditableEntity
{
	public string Name { get; set; } = string.Empty;


	public string? Description { get; set; }

	public Guid? CompanyId { get; set; }

	public int Priority { get; set; }

	public bool IsActive { get; set; } = true;


	public int MaxParallelism { get; set; } = 10;


	public string? RequiredCapabilities { get; set; }
}
