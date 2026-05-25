namespace MADai.Infrastructure.Options;

public class AuditWorkerOptions
{
	public const string SectionName = "AuditWorker";

	public bool Enabled { get; set; } = true;


	public int IntervalMinutes { get; set; } = 15;


	public int FailureThreshold { get; set; } = 3;


	public int DeadLetterThreshold { get; set; } = 10;


	public int OrphanFileAgeHours { get; set; } = 24;

}
