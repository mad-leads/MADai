using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Features.PersistentWorkers;

public interface IRepositoryIntelligenceService
{
	Task<RepositoryIntelligenceDto> GetOrRefreshAsync(RepositoryRegistrationRequest request, bool forceRefresh, CancellationToken cancellationToken = default(CancellationToken));
}
