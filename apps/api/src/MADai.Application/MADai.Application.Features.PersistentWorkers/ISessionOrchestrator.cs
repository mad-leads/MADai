using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Features.PersistentWorkers;

public interface ISessionOrchestrator
{
	Task<InjectTaskResponse> InjectAsync(InjectTaskRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task RotateAsync(SessionRotationRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
