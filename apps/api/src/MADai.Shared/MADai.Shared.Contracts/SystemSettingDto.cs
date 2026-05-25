namespace MADai.Shared.Contracts;

public sealed record SystemSettingDto(string Key, string Category, string? Value, string? DataType, string? Description);
