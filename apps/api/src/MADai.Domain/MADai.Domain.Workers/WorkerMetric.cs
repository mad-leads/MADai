using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class WorkerMetric : Entity
{
	public Guid WorkerNodeId { get; set; }

	public WorkerNode? WorkerNode { get; set; }

	public DateTime Timestamp { get; set; } = DateTime.UtcNow;


	public string MetricName { get; set; } = string.Empty;


	public double Value { get; set; }

	public string? Unit { get; set; }

	public string? Tags { get; set; }
}
