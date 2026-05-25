using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Features.Workers;
using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;

namespace MADai.Infrastructure.Identity;

public class WorkerAuthenticator : IWorkerAuthenticator
{
	private readonly IDbContextAccess _db;

	private readonly IWorkerApiKeyHasher _hasher;

	public WorkerAuthenticator(IDbContextAccess db, IWorkerApiKeyHasher hasher)
	{
		_db = db;
		_hasher = hasher;
	}

	public async Task<WorkerPrincipal?> AuthenticateAsync(string apiKey, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (string.IsNullOrWhiteSpace(apiKey))
		{
			return null;
		}
		string hash = _hasher.Hash(apiKey);
		var worker = await (from w in _db.WorkerNodes
			where w.IsActive && w.ApiKeyHash == hash
			select new { w.Id, w.CompanyId, w.Name }).FirstOrDefaultAsync(cancellationToken);
		return (worker == null) ? null : new WorkerPrincipal(worker.Id, worker.CompanyId, worker.Name);
	}
}
