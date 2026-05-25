using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MADai.Api.Hubs;
using MADai.Application.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace MADai.Api.SignalR;

public class SignalREventPublisher : IEventPublisher
{
	private readonly IHubContext<TasksHub> _tasksHub;

	private readonly IHubContext<WorkersHub> _workersHub;

	private readonly IHubContext<NotificationsHub> _notificationsHub;

	private readonly IHubContext<DashboardHub> _dashboardHub;

	public SignalREventPublisher(IHubContext<TasksHub> tasksHub, IHubContext<WorkersHub> workersHub, IHubContext<NotificationsHub> notificationsHub, IHubContext<DashboardHub> dashboardHub)
	{
		_tasksHub = tasksHub;
		_workersHub = workersHub;
		_notificationsHub = notificationsHub;
		_dashboardHub = dashboardHub;
	}

	public Task PublishTaskUpdatedAsync(Guid companyId, Guid taskId, object payload, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.WhenAll(_tasksHub.Clients.Group($"task:{taskId}").SendAsync("taskUpdated", payload, cancellationToken), _tasksHub.Clients.Group($"company:{companyId}").SendAsync("taskUpdated", payload, cancellationToken));
	}

	public Task PublishWorkerUpdatedAsync(Guid? companyId, Guid workerId, object payload, CancellationToken cancellationToken = default(CancellationToken))
	{
		List<Task> groups = new List<Task> { _workersHub.Clients.All.SendAsync("workerUpdated", payload, cancellationToken) };
		if (companyId.HasValue)
		{
			groups.Add(_workersHub.Clients.Group($"company:{companyId}").SendAsync("workerUpdated", payload, cancellationToken));
		}
		return Task.WhenAll(groups);
	}

	public Task PublishDashboardUpdatedAsync(Guid? companyId, object payload, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (companyId.HasValue)
		{
			return _dashboardHub.Clients.Group($"dashboard:{companyId}").SendAsync("dashboardUpdated", payload, cancellationToken);
		}
		return _dashboardHub.Clients.All.SendAsync("dashboardUpdated", payload, cancellationToken);
	}

	public Task PublishNotificationAsync(Guid? userId, object payload, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (userId.HasValue)
		{
			return _notificationsHub.Clients.Group($"user:{userId}").SendAsync("notification", payload, cancellationToken);
		}
		return _notificationsHub.Clients.All.SendAsync("notification", payload, cancellationToken);
	}

	public Task PublishClaudeTaskUpdatedAsync(Guid taskId, object payload, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _tasksHub.Clients.All.SendAsync("claudeTaskUpdated", payload, cancellationToken);
	}
}
