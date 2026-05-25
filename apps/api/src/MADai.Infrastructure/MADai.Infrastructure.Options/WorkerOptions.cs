using System;

namespace MADai.Infrastructure.Options;

public class WorkerOptions
{
	public const string SectionName = "Worker";

	public string ApiBaseUrl { get; set; } = "http://localhost:5000";


	public string Name { get; set; } = Environment.MachineName;


	public int MaxConcurrency { get; set; } = 2;


	public string Queue { get; set; } = "default";


	public string? ApiKey { get; set; }

	public string? CompanyId { get; set; }

	public string WorkspaceRoot { get; set; } = "workspaces";


	public int PollIntervalSeconds { get; set; } = 5;


	public int HeartbeatIntervalSeconds { get; set; } = 30;

}
