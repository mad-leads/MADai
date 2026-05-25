using System;

namespace MADai.Application.Features.Admin;

public sealed record CompanyAdminDto(Guid Id, string Name, string Slug, string? ContactEmail, bool IsActive, DateTime CreatedDate);
