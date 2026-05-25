namespace MADai.Domain.Enums;

public enum TaskStatus
{
	Draft = 0,
	Queued = 10,
	Scheduled = 15,
	Claimed = 20,
	Running = 30,
	Paused = 35,
	AwaitingValidation = 40,
	Completed = 50,
	Failed = 60,
	Cancelled = 70,
	TimedOut = 80,
	DeadLetter = 90,
	Recovered = 95
}
