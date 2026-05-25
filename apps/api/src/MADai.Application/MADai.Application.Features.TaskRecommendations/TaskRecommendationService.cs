using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.TaskRecommendations;

public class TaskRecommendationService : ITaskRecommendationService
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	private readonly IDateTimeProvider _clock;

	public TaskRecommendationService(IDbContextAccess db, ICurrentUserService currentUser, IDateTimeProvider clock)
	{
		_db = db;
		_currentUser = currentUser;
		_clock = clock;
	}

	public async Task<IReadOnlyList<TaskRecommendationDto>> ListAsync(Guid? taskId, CancellationToken cancellationToken = default(CancellationToken))
	{
		IQueryable<TaskRecommendation> q = _db.TaskRecommendations.AsNoTracking().AsQueryable();
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			q = q.Where((TaskRecommendation r) => r.CompanyId == cid);
		}
		if (taskId.HasValue)
		{
			Guid id = taskId.GetValueOrDefault();
			q = q.Where((TaskRecommendation r) => r.TaskId == id);
		}
		return await (from r in q
			orderby r.CreatedDate descending
			select new TaskRecommendationDto(r.Id, r.TaskId, r.Title, r.Body, r.Source, r.Status, r.Confidence, r.AppliedAt, r.CreatedDate)).ToListAsync(cancellationToken);
	}

	public async Task<TaskRecommendationDto> ApplyAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaskRecommendation entity = await Find(id, cancellationToken);
		entity.Status = "Applied";
		entity.AppliedAt = _clock.UtcNow;
		await _db.SaveChangesAsync(cancellationToken);
		return new TaskRecommendationDto(entity.Id, entity.TaskId, entity.Title, entity.Body, entity.Source, entity.Status, entity.Confidence, entity.AppliedAt, entity.CreatedDate);
	}

	public async Task<TaskRecommendationDto> DismissAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaskRecommendation entity = await Find(id, cancellationToken);
		entity.Status = "Dismissed";
		await _db.SaveChangesAsync(cancellationToken);
		return new TaskRecommendationDto(entity.Id, entity.TaskId, entity.Title, entity.Body, entity.Source, entity.Status, entity.Confidence, entity.AppliedAt, entity.CreatedDate);
	}

	private async Task<TaskRecommendation> Find(Guid id, CancellationToken cancellationToken)
	{
		TaskRecommendation entity = (await _db.TaskRecommendations.FirstOrDefaultAsync((TaskRecommendation r) => r.Id == id, cancellationToken)) ?? throw new NotFoundException("TaskRecommendation", id);
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
}
