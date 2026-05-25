using System.Security.Claims;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;

namespace MADai.Api.Authentication;

public sealed class HangfireDashboardAuthFilter : IDashboardAuthorizationFilter
{
	public bool Authorize(DashboardContext context)
	{
		HttpContext http = context.GetHttpContext();
		ClaimsPrincipal user = http.User;
		if (user != null && (user.Identity?.IsAuthenticated).GetValueOrDefault())
		{
			return http.User.IsInRole("SystemAdmin");
		}
		return false;
	}
}
