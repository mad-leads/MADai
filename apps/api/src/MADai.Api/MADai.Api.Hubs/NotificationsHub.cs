using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MADai.Api.Hubs;

[Authorize]
public class NotificationsHub : Hub
{
	public override Task OnConnectedAsync()
	{
		string userId = base.Context.User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? base.Context.User?.FindFirst("sub")?.Value;
		if (Guid.TryParse(userId, out var _))
		{
			return base.Groups.AddToGroupAsync(base.Context.ConnectionId, "user:" + userId);
		}
		return base.OnConnectedAsync();
	}
}
