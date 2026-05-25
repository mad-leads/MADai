namespace MADai.Application.Features.Admin;

public sealed record UpsertFeatureFlagRequest(string Key, string Name, string? Description, bool IsEnabled, string? Audience, string? Configuration);
