using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MADai.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace MADai.Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _accessor;

	private ClaimsPrincipal? User => _accessor.HttpContext?.User;

	public Guid? UserId
	{
		get
		{
			if (!Guid.TryParse(User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? User?.FindFirst("sub")?.Value, out var id))
			{
				return null;
			}
			return id;
		}
	}

	public string? Email
	{
		get
		{
			object obj = User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
			if (obj == null)
			{
				ClaimsPrincipal? user = User;
				if (user == null)
				{
					return null;
				}
				Claim? claim = user.FindFirst("email");
				if (claim == null)
				{
					return null;
				}
				obj = claim.Value;
			}
			return (string?)obj;
		}
	}

	public Guid? CompanyId
	{
		get
		{
			if (!Guid.TryParse(User?.FindFirst("company_id")?.Value, out var id))
			{
				return null;
			}
			return id;
		}
	}

	public IReadOnlyList<string> Roles => (from c in User?.FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
		select c.Value).ToList() ?? new List<string>();

	public IReadOnlyList<string> Permissions => (from c in User?.FindAll("permission")
		select c.Value).ToList() ?? new List<string>();

	public bool IsAuthenticated
	{
		get
		{
			ClaimsPrincipal? user = User;
			if (user == null)
			{
				return false;
			}
			return (user.Identity?.IsAuthenticated).GetValueOrDefault();
		}
	}

	public CurrentUserService(IHttpContextAccessor accessor)
	{
		_accessor = accessor;
	}

	public bool IsInRole(string role)
	{
		return User?.IsInRole(role) ?? false;
	}

	public bool HasPermission(string permission)
	{
		return Permissions.Contains(permission);
	}
}
