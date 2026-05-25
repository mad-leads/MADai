using System;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;

namespace MADai.Infrastructure.Events;

public class NullEventPublisher : IEventPublisher
{
	public Task PublishTaskUpdatedAsync(Guid companyId, Guid taskId, object payload, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.CompletedTask;
	}

	public Task PublishWorkerUpdatedAsync(Guid? companyId, Guid workerId, object payload, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.CompletedTask;
	}

	public Task PublishDashboardUpdatedAsync(Guid? companyId, object payload, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.CompletedTask;
	}

	public Task PublishNotificationAsync(Guid? userId, object payload, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.CompletedTask;
	}

	public Task PublishClaudeTaskUpdatedAsync(Guid taskId, object payload, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.CompletedTask;
	}
}
