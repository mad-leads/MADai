namespace MADai.Application.Features.Admin;

public sealed record UpsertCompanyRequest(string Name, string Slug, string? ContactEmail, bool IsActive);
