using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MADai.Domain.Identity;

namespace MADai.Application.Abstractions;

public interface IJwtTokenService
{
	Task<(string AccessToken, DateTime ExpiresAt)> GenerateAccessTokenAsync(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions, CancellationToken cancellationToken = default(CancellationToken));

	string GenerateRefreshToken();
}
