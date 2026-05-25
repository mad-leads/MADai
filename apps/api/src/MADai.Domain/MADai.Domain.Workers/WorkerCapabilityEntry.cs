using System;
using MADai.Domain.Common;
using MADai.Domain.Enums;

namespace MADai.Domain.Workers;

public class WorkerCapabilityEntry : Entity
{
	public Guid WorkerNodeId { get; set; }

	public WorkerNode? WorkerNode { get; set; }

	public WorkerCapability Capability { get; set; }

	public string? Detail { get; set; }
}
