namespace MADai.Shared.Contracts;

public sealed record MfaEnrollResponse(string SharedKey, string AuthenticatorUri);
