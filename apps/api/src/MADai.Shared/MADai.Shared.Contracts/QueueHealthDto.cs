namespace MADai.Shared.Contracts;

public sealed record QueueHealthDto(string Name, long Pending, long Running, long Failed, double AvgWaitSeconds);
