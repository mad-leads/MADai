using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.SystemEntities;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Webhooks;

public class WebhookService : IWebhookService
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	public WebhookService(IDbContextAccess db, ICurrentUserService currentUser)
	{
		_db = db;
		_currentUser = currentUser;
	}

	public async Task<IReadOnlyList<WebhookEndpointDto>> ListAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		IQueryable<WebhookEndpoint> q = _db.WebhookEndpoints.AsNoTracking().AsQueryable();
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			q = q.Where((WebhookEndpoint w) => w.CompanyId == cid);
		}
		return await (from w in q
			orderby w.Url
			select new WebhookEndpointDto(w.Id, w.Url, w.EventsCsv, w.IsActive, w.LastSuccessAt, w.LastFailureAt, w.ConsecutiveFailures, w.CreatedDate)).ToListAsync(cancellationToken);
	}

	public async Task<WebhookEndpointDto> CreateAsync(CreateWebhookEndpointRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		Guid companyId = _currentUser.CompanyId ?? throw new ForbiddenException();
		if (!Uri.TryCreate(request.Url, UriKind.Absolute, out Uri _))
		{
			throw new AppException("Url must be an absolute URI.");
		}
		WebhookEndpoint entity = new WebhookEndpoint
		{
			CompanyId = companyId,
			Url = request.Url,
			EventsCsv = request.EventsCsv,
			IsActive = request.IsActive,
			Secret = GenerateSecret()
		};
		_db.WebhookEndpoints.Add(entity);
		await _db.SaveChangesAsync(cancellationToken);
		return Map(entity);
	}

	public async Task<WebhookEndpointDto> UpdateAsync(Guid id, UpdateWebhookEndpointRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		WebhookEndpoint entity = await Find(id, cancellationToken);
		if (request.Url != null)
		{
			if (!Uri.TryCreate(request.Url, UriKind.Absolute, out Uri _))
			{
				throw new AppException("Url must be an absolute URI.");
			}
			entity.Url = request.Url;
		}
		if (request.EventsCsv != null)
		{
			entity.EventsCsv = request.EventsCsv;
		}
		bool? isActive = request.IsActive;
		if (isActive.HasValue)
		{
			bool a = isActive.GetValueOrDefault();
			entity.IsActive = a;
		}
		await _db.SaveChangesAsync(cancellationToken);
		return Map(entity);
	}

	public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		WebhookEndpoint entity = await Find(id, cancellationToken);
		_db.WebhookEndpoints.Remove(entity);
		await _db.SaveChangesAsync(cancellationToken);
	}

	public async Task<string> RotateSecretAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		WebhookEndpoint entity = await Find(id, cancellationToken);
		entity.Secret = GenerateSecret();
		await _db.SaveChangesAsync(cancellationToken);
		return entity.Secret;
	}

	private async Task<WebhookEndpoint> Find(Guid id, CancellationToken cancellationToken)
	{
		WebhookEndpoint entity = (await _db.WebhookEndpoints.FirstOrDefaultAsync((WebhookEndpoint w) => w.Id == id, cancellationToken)) ?? throw new NotFoundException("WebhookEndpoint", id);
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			if (entity.CompanyId != cid)
			{
				throw new ForbiddenException();
			}
		}
		return entity;
	}

	private static string GenerateSecret()
	{
		return Convert.ToHexString(RandomNumberGenerator.GetBytes(32)).ToLowerInvariant();
	}

	private static WebhookEndpointDto Map(WebhookEndpoint w)
	{
		return new WebhookEndpointDto(w.Id, w.Url, w.EventsCsv, w.IsActive, w.LastSuccessAt, w.LastFailureAt, w.ConsecutiveFailures, w.CreatedDate);
	}
}
