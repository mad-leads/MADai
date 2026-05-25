using System;
using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Abstractions;

public interface IEventPublisher
{
	Task PublishTaskUpdatedAsync(Guid companyId, Guid taskId, object payload, CancellationToken cancellationToken = default(CancellationToken));

	Task PublishWorkerUpdatedAsync(Guid? companyId, Guid workerId, object payload, CancellationToken cancellationToken = default(CancellationToken));

	Task PublishDashboardUpdatedAsync(Guid? companyId, object payload, CancellationToken cancellationToken = default(CancellationToken));

	Task PublishNotificationAsync(Guid? userId, object payload, CancellationToken cancellationToken = default(CancellationToken));

	Task PublishClaudeTaskUpdatedAsync(Guid taskId, object payload, CancellationToken cancellationToken = default(CancellationToken));
}
