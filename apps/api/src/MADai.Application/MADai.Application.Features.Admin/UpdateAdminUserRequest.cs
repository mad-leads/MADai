using System;
using System.Collections.Generic;

namespace MADai.Application.Features.Admin;

public sealed record UpdateAdminUserRequest(string? FirstName, string? LastName, bool? IsActive, Guid? CompanyId, IReadOnlyList<string>? Roles);
