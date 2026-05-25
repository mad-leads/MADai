using System;
using System.Collections.Generic;

namespace MADai.Shared.Contracts;

public sealed record UserSummary(Guid Id, string Email, string DisplayName, Guid? CompanyId, string? CompanyName, IReadOnlyList<string> Roles, IReadOnlyList<string> Permissions, bool IsMfaEnrolled, string? AvatarUrl, string? FirstName = null, string? LastName = null, string? TimeZone = null, string? Locale = null);
