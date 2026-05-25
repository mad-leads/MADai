namespace MADai.Shared.Contracts;

public sealed record LoginRequest(string Email, string Password, string? MfaCode);
