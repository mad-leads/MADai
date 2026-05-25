namespace MADai.Shared.Contracts;

public sealed record UpdateProfileRequest(string? FirstName, string? LastName, string? AvatarUrl, string? TimeZone, string? Locale);
