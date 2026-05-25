using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MADai.Shared.Contracts;

namespace MADai.Application.Features.Dashboard;

public interface IDashboardService
{
	Task<SystemOverviewDto> GetOverviewAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<IReadOnlyList<QueueHealthDto>> GetQueueHealthAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<IReadOnlyList<WorkerHealthDto>> GetWorkerHealthAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<IReadOnlyList<FailureTrendPointDto>> GetFailureTrendAsync(int days, CancellationToken cancellationToken = default(CancellationToken));

	Task<IReadOnlyList<CompletionTrendPointDto>> GetCompletionTrendAsync(int days, CancellationToken cancellationToken = default(CancellationToken));
}
