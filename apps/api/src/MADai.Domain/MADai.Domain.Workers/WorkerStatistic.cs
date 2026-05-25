using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class WorkerStatistic : Entity
{
	public Guid? WorkerNodeId { get; set; }

	public string RepositoryKey { get; set; } = string.Empty;


	public DateTime Timestamp { get; set; } = DateTime.UtcNow;


	public string MetricName { get; set; } = string.Empty;


	public double Value { get; set; }

	public string? Unit { get; set; }
}
