using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Features.Admin;

public interface IPlanAdminService
{
	Task<IReadOnlyList<PlanDto>> ListAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<PlanDto> UpsertAsync(UpsertPlanRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
}
