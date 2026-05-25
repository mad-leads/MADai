using System;
using System.Collections.Generic;
using MADai.Domain.Common;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;

namespace MADai.Domain.Workers;

public class WorkerNode : AuditableEntity
{
	public string Name { get; set; } = string.Empty;


	public string MachineName { get; set; } = string.Empty;


	public string? AgentVersion { get; set; }

	public string? OperatingSystem { get; set; }

	public string? IpAddress { get; set; }

	public WorkerStatus Status { get; set; }

	public Guid? CompanyId { get; set; }

	public string? QueueName { get; set; }

	public int MaxConcurrency { get; set; } = 1;


	public int CurrentConcurrency { get; set; }

	public string? Capabilities { get; set; }

	public string? Labels { get; set; }

	public DateTime? LastHeartbeatAt { get; set; }

	public DateTime? StartedAt { get; set; }

	public string ApiKeyHash { get; set; } = string.Empty;


	public bool IsActive { get; set; } = true;


	public string? WorkspaceRoot { get; set; }

	public ICollection<WorkerHeartbeat> Heartbeats { get; set; } = new List<WorkerHeartbeat>();


	public ICollection<WorkerMetric> Metrics { get; set; } = new List<WorkerMetric>();


	public ICollection<TaskItem> ClaimedTasks { get; set; } = new List<TaskItem>();

}
