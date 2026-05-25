using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MADai.Api.Hubs;

[Authorize]
public class DashboardHub : Hub
{
	public Task JoinCompany(string companyId)
	{
		if (!Guid.TryParse(companyId, out var _))
		{
			return Task.CompletedTask;
		}
		return base.Groups.AddToGroupAsync(base.Context.ConnectionId, "dashboard:" + companyId);
	}
}
