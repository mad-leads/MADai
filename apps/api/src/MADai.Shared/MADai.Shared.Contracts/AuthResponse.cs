using System;

namespace MADai.Shared.Contracts;

public sealed record AuthResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt, UserSummary User);
