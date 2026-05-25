namespace MADai.Shared.Contracts;

public static class MADaiConstants
{
	public const string DefaultQueue = "default";

	public const string AuditQueue = "audit";

	public const string MediaQueue = "media";

	public const string CodeQueue = "code";

	public const string CompanyHeader = "X-Company-Id";

	public const string ApiKeyHeader = "X-API-Key";

	public const string CorrelationHeader = "X-Correlation-Id";

	public const string WorkerIdHeader = "X-Worker-Id";

	public const string SignalRTasksHub = "/hubs/tasks";

	public const string SignalRWorkersHub = "/hubs/workers";

	public const string SignalRNotificationsHub = "/hubs/notifications";

	public const string SignalRDashboardHub = "/hubs/dashboard";

	public const string AuthSchemeJwt = "Bearer";

	public const string AuthSchemeApiKey = "ApiKey";

	public const string AuthSchemeWorker = "Worker";

	public const string DefaultStorageRoot = "storage";

	public const string DefaultWorkspaceRoot = "workspaces";
}
