namespace MADai.Shared.Contracts;

public sealed record TaskProgressReport(int Progress, string? StatusMessage, string? CheckpointJson);
