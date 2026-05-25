using System;
using System.Threading;
using System.Threading.Tasks;
using MADai.Shared.Contracts;

namespace MADai.Application.Features.Workers;

public interface IWorkerRegistrationService
{
	Task<WorkerRegisterResponse> RegisterAsync(WorkerRegisterRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task DeactivateAsync(Guid workerId, CancellationToken cancellationToken = default(CancellationToken));

	Task DrainAsync(Guid workerId, CancellationToken cancellationToken = default(CancellationToken));
}
