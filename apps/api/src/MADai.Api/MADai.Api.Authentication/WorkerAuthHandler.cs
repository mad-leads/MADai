using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MADai.Application.Features.Workers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MADai.Api.Authentication;

public class WorkerAuthHandler : AuthenticationHandler<WorkerAuthOptions>
{
	private readonly IWorkerAuthenticator _workerAuth;

	public WorkerAuthHandler(IOptionsMonitor<WorkerAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, IWorkerAuthenticator workerAuth)
		: base(options, logger, encoder)
	{
		_workerAuth = workerAuth;
	}

	protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		if (!base.Request.Headers.TryGetValue("X-API-Key", out var apiKey) || string.IsNullOrWhiteSpace(apiKey))
		{
			return AuthenticateResult.NoResult();
		}
		WorkerPrincipal principal = await _workerAuth.AuthenticateAsync(apiKey.ToString(), base.Context.RequestAborted);
		if ((object)principal == null)
		{
			return AuthenticateResult.Fail("Invalid worker key.");
		}
		List<Claim> claims = new List<Claim>
		{
			new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", principal.WorkerId.ToString()),
			new Claim("worker_id", principal.WorkerId.ToString()),
			new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", principal.Name),
			new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Worker")
		};
		if (principal.CompanyId.HasValue)
		{
			claims.Add(new Claim("company_id", principal.CompanyId.Value.ToString()));
		}
		ClaimsIdentity identity = new ClaimsIdentity(claims, base.Scheme.Name);
		ClaimsPrincipal p = new ClaimsPrincipal(identity);
		AuthenticationTicket ticket = new AuthenticationTicket(p, base.Scheme.Name);
		return AuthenticateResult.Success(ticket);
	}
}
