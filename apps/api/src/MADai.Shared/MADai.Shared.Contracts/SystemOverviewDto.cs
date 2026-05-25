namespace MADai.Shared.Contracts;

public sealed record SystemOverviewDto(long TotalTasks, long QueuedTasks, long RunningTasks, long CompletedToday, long FailedToday, long DeadLetterCount, int ActiveWorkers, int IdleWorkers, int OfflineWorkers, double AverageDurationSeconds, double SuccessRatePercent);
