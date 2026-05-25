using System;
using MADai.Domain.Common;
using MADai.Domain.Enums;

namespace MADai.Domain.Workers;

public class WorkerHeartbeat : Entity
{
	public Guid WorkerNodeId { get; set; }

	public WorkerNode? WorkerNode { get; set; }

	public DateTime Timestamp { get; set; } = DateTime.UtcNow;


	public WorkerStatus Status { get; set; }

	public int ActiveTasks { get; set; }

	public double CpuPercent { get; set; }

	public double MemoryMb { get; set; }

	public double DiskFreeGb { get; set; }

	public string? Notes { get; set; }
}
