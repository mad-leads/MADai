using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MADai.Shared.Contracts;

namespace MADai.Application.Features.Workers;

public interface IWorkerQueueService
{
	Task<IReadOnlyList<TaskClaimResponseItem>> ClaimAsync(Guid workerId, TaskClaimRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task ReportProgressAsync(Guid workerId, Guid taskId, string claimToken, TaskProgressReport report, CancellationToken cancellationToken = default(CancellationToken));

	Task ReportLogAsync(Guid workerId, Guid taskId, string claimToken, TaskLogEntry entry, CancellationToken cancellationToken = default(CancellationToken));

	Task CompleteAsync(Guid workerId, Guid taskId, string claimToken, TaskCompletionReport report, CancellationToken cancellationToken = default(CancellationToken));

	Task FailAsync(Guid workerId, Guid taskId, string claimToken, TaskFailureReport report, CancellationToken cancellationToken = default(CancellationToken));

	Task HeartbeatAsync(Guid workerId, WorkerHeartbeatRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
