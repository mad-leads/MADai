using System;
using MADai.Domain.Common;

namespace MADai.Domain.Tenancy;

public class CompanySettings : Entity
{
	public Guid CompanyId { get; set; }

	public Company? Company { get; set; }

	public int MaxConcurrentTasks { get; set; } = 25;


	public int MaxStorageGb { get; set; } = 50;


	public int MaxWorkers { get; set; } = 5;


	public int TaskRetentionDays { get; set; } = 90;


	public bool EnableSelfHealing { get; set; } = true;


	public bool EnableAutoRetry { get; set; } = true;


	public int DefaultTaskTimeoutMinutes { get; set; } = 60;


	public string? WebhookSecret { get; set; }

	public string? CustomDomain { get; set; }
}
