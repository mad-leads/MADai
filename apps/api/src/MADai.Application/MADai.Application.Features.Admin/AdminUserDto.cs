using System;
using System.Collections.Generic;

namespace MADai.Application.Features.Admin;

public sealed record AdminUserDto(Guid Id, string Email, string? FirstName, string? LastName, bool IsActive, bool IsMfaEnrolled, Guid? CompanyId, string? CompanyName, IReadOnlyList<string> Roles, DateTime? LastLoginAt, DateTime CreatedDate);
