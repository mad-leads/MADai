using System;
using System.Collections.Generic;

namespace MADai.Application.Abstractions;

public interface ICurrentUserService
{
	Guid? UserId { get; }

	string? Email { get; }

	Guid? CompanyId { get; }

	IReadOnlyList<string> Roles { get; }

	IReadOnlyList<string> Permissions { get; }

	bool IsAuthenticated { get; }

	bool IsInRole(string role);

	bool HasPermission(string permission);
}
