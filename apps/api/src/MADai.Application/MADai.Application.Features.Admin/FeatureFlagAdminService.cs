using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.SystemEntities;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Admin;

public class FeatureFlagAdminService : IFeatureFlagAdminService
{
	private readonly IDbContextAccess _db;

	public FeatureFlagAdminService(IDbContextAccess db)
	{
		_db = db;
	}

	public async Task<IReadOnlyList<FeatureFlagDto>> ListAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return await (from f in _db.FeatureFlags.AsNoTracking()
			orderby f.Key
			select new FeatureFlagDto(f.Id, f.Key, f.Name, f.Description, f.IsEnabled, f.Audience, f.Configuration)).ToListAsync(cancellationToken);
	}

	public async Task<FeatureFlagDto> UpsertAsync(UpsertFeatureFlagRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		UpsertFeatureFlagRequest request2 = request;
		if (string.IsNullOrWhiteSpace(request2.Key))
		{
			throw new AppException("Key required.");
		}
		FeatureFlag existing = await _db.FeatureFlags.FirstOrDefaultAsync((FeatureFlag f) => f.Key == request2.Key, cancellationToken);
		if (existing == null)
		{
			existing = new FeatureFlag
			{
				Key = request2.Key.Trim(),
				Name = request2.Name,
				Description = request2.Description,
				IsEnabled = request2.IsEnabled,
				Audience = request2.Audience,
				Configuration = request2.Configuration
			};
			_db.FeatureFlags.Add(existing);
		}
		else
		{
			existing.Name = request2.Name;
			existing.Description = request2.Description;
			existing.IsEnabled = request2.IsEnabled;
			existing.Audience = request2.Audience;
			existing.Configuration = request2.Configuration;
		}
		await _db.SaveChangesAsync(cancellationToken);
		return new FeatureFlagDto(existing.Id, existing.Key, existing.Name, existing.Description, existing.IsEnabled, existing.Audience, existing.Configuration);
	}

	public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		FeatureFlag entity = (await _db.FeatureFlags.FirstOrDefaultAsync((FeatureFlag f) => f.Id == id, cancellationToken)) ?? throw new NotFoundException("FeatureFlag", id);
		_db.FeatureFlags.Remove(entity);
		await _db.SaveChangesAsync(cancellationToken);
	}
}
