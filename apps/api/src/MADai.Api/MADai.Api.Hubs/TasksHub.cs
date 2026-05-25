using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MADai.Api.Hubs;

[Authorize]
public class TasksHub : Hub
{
	public async Task JoinCompany(string companyId)
	{
		if (Guid.TryParse(companyId, out var _))
		{
			await base.Groups.AddToGroupAsync(base.Context.ConnectionId, "company:" + companyId);
		}
	}

	public async Task JoinTask(string taskId)
	{
		if (Guid.TryParse(taskId, out var _))
		{
			await base.Groups.AddToGroupAsync(base.Context.ConnectionId, "task:" + taskId);
		}
	}

	public Task LeaveTask(string taskId)
	{
		return base.Groups.RemoveFromGroupAsync(base.Context.ConnectionId, "task:" + taskId);
	}
}
