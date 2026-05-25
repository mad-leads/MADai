namespace MADai.Shared.Contracts;

public sealed record ResetPasswordRequest(string Email, string Token, string NewPassword);
