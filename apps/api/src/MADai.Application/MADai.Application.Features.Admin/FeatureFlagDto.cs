using System;

namespace MADai.Application.Features.Admin;

public sealed record FeatureFlagDto(Guid Id, string Key, string Name, string? Description, bool IsEnabled, string? Audience, string? Configuration);
