namespace MADai.Domain.Enums;

public enum AuditScanType
{
	FailedTasks = 1,
	IncompleteTasks,
	OrphanedFiles,
	LogErrors,
	QueueBacklog,
	PerformanceBottleneck,
	SecurityConcern,
	DatabaseGrowth,
	DeploymentFailure,
	RetryPattern,
	UserComplaint,
	TodoComment
}
