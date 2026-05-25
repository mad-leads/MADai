namespace MADai.Shared.Contracts;

public sealed record TaskFailureReport(string Reason, string? StackTrace, bool IsTransient);
