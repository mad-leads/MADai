namespace MADai.Domain.Enums;

public enum WorkerStatus
{
	Offline = 0,
	Starting = 10,
	Idle = 20,
	Busy = 30,
	Draining = 40,
	Errored = 50,
	Maintenance = 60
}
