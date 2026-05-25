using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Features.Admin;

public interface IFeatureFlagAdminService
{
	Task<IReadOnlyList<FeatureFlagDto>> ListAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<FeatureFlagDto> UpsertAsync(UpsertFeatureFlagRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
}
