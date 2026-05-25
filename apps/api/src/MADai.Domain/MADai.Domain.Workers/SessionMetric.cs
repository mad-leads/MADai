using System;
using MADai.Domain.Common;

namespace MADai.Domain.Workers;

public class SessionMetric : Entity
{
	public string SessionId { get; set; } = string.Empty;


	public Guid? WorkerNodeId { get; set; }

	public string RepositoryKey { get; set; } = string.Empty;


	public DateTime Timestamp { get; set; } = DateTime.UtcNow;


	public int ActiveTaskCount { get; set; }

	public int EstimatedTokens { get; set; }

	public double MemoryMb { get; set; }

	public string Health { get; set; } = "Healthy";


	public string? Notes { get; set; }
}
