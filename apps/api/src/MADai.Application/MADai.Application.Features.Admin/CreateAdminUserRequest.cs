using System;
using System.Collections.Generic;

namespace MADai.Application.Features.Admin;

public sealed record CreateAdminUserRequest(string Email, string Password, string? FirstName, string? LastName, Guid? CompanyId, IReadOnlyList<string> Roles);
