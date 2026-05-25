namespace MADai.Shared.Contracts;

public sealed record TaskCompletionReport(string? OutputSummary, string? ResultPayload, string? ValidationReport, bool ValidationPassed);
