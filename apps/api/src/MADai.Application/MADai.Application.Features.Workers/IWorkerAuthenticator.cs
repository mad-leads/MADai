using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Features.Workers;

public interface IWorkerAuthenticator
{
	Task<WorkerPrincipal?> AuthenticateAsync(string apiKey, CancellationToken cancellationToken = default(CancellationToken));
}
