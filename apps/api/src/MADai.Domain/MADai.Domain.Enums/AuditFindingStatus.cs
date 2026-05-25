namespace MADai.Domain.Enums;

public enum AuditFindingStatus
{
	Open = 0,
	Acknowledged = 10,
	InProgress = 20,
	Resolved = 30,
	Ignored = 40,
	Reopened = 50
}
