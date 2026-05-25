using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MADai.Application.Features.Admin;

public interface ICompanyAdminService
{
	Task<IReadOnlyList<CompanyAdminDto>> ListAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task<CompanyAdminDto> CreateAsync(UpsertCompanyRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task<CompanyAdminDto> UpdateAsync(Guid id, UpsertCompanyRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
}
