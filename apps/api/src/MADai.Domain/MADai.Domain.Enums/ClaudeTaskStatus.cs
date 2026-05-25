namespace MADai.Domain.Enums;

public enum ClaudeTaskStatus
{
	Pending = 0,
	InProgress = 10,
	ToBeDeployed = 20,
	Completed = 30,
	Failed = 40,
	Cancelled = 50,
	Deferred = 60
}
