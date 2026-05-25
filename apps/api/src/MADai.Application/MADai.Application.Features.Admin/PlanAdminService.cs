using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Billing;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Admin;

public class PlanAdminService : IPlanAdminService
{
	private readonly IDbContextAccess _db;

	public PlanAdminService(IDbContextAccess db)
	{
		_db = db;
	}

	public async Task<IReadOnlyList<PlanDto>> ListAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return await (from p in _db.Plans.AsNoTracking()
			orderby p.MonthlyPrice
			select new PlanDto(p.Id, p.Name, p.Code, p.Description, p.MonthlyPrice, p.AnnualPrice, p.Currency, p.IncludedTasks, p.IncludedStorageGb, p.IncludedWorkers, p.IsPublic, p.IsActive)).ToListAsync(cancellationToken);
	}

	public async Task<PlanDto> UpsertAsync(UpsertPlanRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		UpsertPlanRequest request2 = request;
		if (string.IsNullOrWhiteSpace(request2.Code))
		{
			throw new AppException("Code required.");
		}
		Plan entity = await _db.Plans.FirstOrDefaultAsync((Plan p) => p.Code == request2.Code, cancellationToken);
		if (entity == null)
		{
			entity = new Plan
			{
				Code = request2.Code.Trim()
			};
			_db.Plans.Add(entity);
		}
		entity.Name = request2.Name;
		entity.Description = request2.Description;
		entity.MonthlyPrice = request2.MonthlyPrice;
		entity.AnnualPrice = request2.AnnualPrice;
		entity.Currency = request2.Currency;
		entity.IncludedTasks = request2.IncludedTasks;
		entity.IncludedStorageGb = request2.IncludedStorageGb;
		entity.IncludedWorkers = request2.IncludedWorkers;
		entity.IsPublic = request2.IsPublic;
		entity.IsActive = request2.IsActive;
		await _db.SaveChangesAsync(cancellationToken);
		return new PlanDto(entity.Id, entity.Name, entity.Code, entity.Description, entity.MonthlyPrice, entity.AnnualPrice, entity.Currency, entity.IncludedTasks, entity.IncludedStorageGb, entity.IncludedWorkers, entity.IsPublic, entity.IsActive);
	}

	public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		Plan entity = (await _db.Plans.FirstOrDefaultAsync((Plan p) => p.Id == id, cancellationToken)) ?? throw new NotFoundException("Plan", id);
		_db.Plans.Remove(entity);
		await _db.SaveChangesAsync(cancellationToken);
	}
}
