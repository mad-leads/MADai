using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.Identity;
using MADai.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MADai.Infrastructure.Identity;

public class JwtTokenService : IJwtTokenService
{
	private readonly JwtOptions _options;

	public JwtTokenService(IOptions<JwtOptions> options)
	{
		_options = options.Value;
	}

	public Task<(string AccessToken, DateTime ExpiresAt)> GenerateAccessTokenAsync(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions, CancellationToken cancellationToken = default(CancellationToken))
	{
		SigningCredentials credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret)), "HS256");
		List<Claim> claims = new List<Claim>
		{
			new Claim("sub", user.Id.ToString()),
			new Claim("email", user.Email ?? string.Empty),
			new Claim("jti", Guid.NewGuid().ToString("N")),
			new Claim("name", user.DisplayName)
		};
		if (user.CompanyId.HasValue)
		{
			claims.Add(new Claim("company_id", user.CompanyId.Value.ToString()));
		}
		foreach (string role in roles.Distinct())
		{
			claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role));
		}
		foreach (string perm in permissions.Distinct())
		{
			claims.Add(new Claim("permission", perm));
		}
		DateTime expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenLifetimeMinutes);
		JwtSecurityToken token = new JwtSecurityToken(_options.Issuer, _options.Audience, claims, DateTime.UtcNow, expires, credentials);
		return Task.FromResult((new JwtSecurityTokenHandler().WriteToken(token), expires));
	}

	public string GenerateRefreshToken()
	{
		return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).Replace("+", "-").Replace("/", "_")
			.TrimEnd('=');
	}
}
