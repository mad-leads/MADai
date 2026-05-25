using System;
using System.Threading;
using System.Threading.Tasks;
using MADai.Shared.Contracts;

namespace MADai.Application.Features.Admin;

public interface IAdminUserService
{
	Task<PagedResult<AdminUserDto>> ListAsync(PageQuery query, CancellationToken cancellationToken = default(CancellationToken));

	Task<AdminUserDto> GetAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

	Task<AdminUserDto> CreateAsync(CreateAdminUserRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task<AdminUserDto> UpdateAsync(Guid id, UpdateAdminUserRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task SetPasswordAsync(Guid id, string newPassword, CancellationToken cancellationToken = default(CancellationToken));

	Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
}
