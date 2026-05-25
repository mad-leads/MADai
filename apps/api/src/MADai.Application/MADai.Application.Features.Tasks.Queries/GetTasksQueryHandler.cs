using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Tasks.Queries;

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, PagedResult<TaskSummaryDto>>
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	public GetTasksQueryHandler(IDbContextAccess db, ICurrentUserService currentUser)
	{
		_db = db;
		_currentUser = currentUser;
	}

	public async Task<PagedResult<TaskSummaryDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
	{
		IQueryable<TaskItem> query2 = _db.Tasks.AsNoTracking().AsQueryable();
		Guid? companyId2 = _currentUser.CompanyId;
		if (companyId2.HasValue)
		{
			Guid companyId = companyId2.GetValueOrDefault();
			query2 = query2.Where((TaskItem t) => t.CompanyId == companyId);
		}
		TaskQueryRequest r = request.Request.Normalize();
		MADai.Domain.Enums.TaskStatus[] statuses = r.Statuses;
		if (statuses != null && statuses.Length > 0)
		{
			query2 = query2.Where((TaskItem t) => statuses.Contains(t.Status));
		}
		TaskCategory[] categories = r.Categories;
		if (categories != null && categories.Length > 0)
		{
			query2 = query2.Where((TaskItem t) => categories.Contains(t.Category));
		}
		TaskPriority? minPriority = r.MinPriority;
		if (minPriority.HasValue)
		{
			TaskPriority minP = minPriority.GetValueOrDefault();
			query2 = query2.Where((TaskItem t) => (int)t.Priority >= (int)minP);
		}
		companyId2 = r.ClaimedByWorkerId;
		if (companyId2.HasValue)
		{
			Guid workerId = companyId2.GetValueOrDefault();
			query2 = query2.Where((TaskItem t) => t.ClaimedByWorkerId == workerId);
		}
		DateTime? createdFrom = r.CreatedFrom;
		if (createdFrom.HasValue)
		{
			DateTime from = createdFrom.GetValueOrDefault();
			query2 = query2.Where((TaskItem t) => t.CreatedDate >= from);
		}
		createdFrom = r.CreatedTo;
		if (createdFrom.HasValue)
		{
			DateTime to = createdFrom.GetValueOrDefault();
			query2 = query2.Where((TaskItem t) => t.CreatedDate <= to);
		}
		companyId2 = r.ParentTaskId;
		if (companyId2.HasValue)
		{
			Guid parent = companyId2.GetValueOrDefault();
			query2 = query2.Where((TaskItem t) => t.ParentTaskId == parent);
		}
		if (!string.IsNullOrWhiteSpace(r.Queue))
		{
			query2 = query2.Where((TaskItem t) => t.QueueName == r.Queue);
		}
		bool? isDeadLetter = r.IsDeadLetter;
		if (isDeadLetter.HasValue)
		{
			bool dlq = isDeadLetter.GetValueOrDefault();
			query2 = query2.Where((TaskItem t) => t.IsDeadLetter == dlq);
		}
		if (!string.IsNullOrWhiteSpace(r.Search))
		{
			string term = r.Search.Trim();
			query2 = query2.Where((TaskItem t) => EF.Functions.Like(t.Title, $"%{term}%") || (t.Description != null && EF.Functions.Like(t.Description, $"%{term}%")));
		}
		string text = r.SortBy?.ToLowerInvariant();
		bool sortDescending = r.SortDescending;
		query2 = text switch
		{
			"priority" => sortDescending ? (from t in query2
				orderby t.Priority descending, t.CreatedDate descending
				select t) : (from t in query2
				orderby t.Priority, t.CreatedDate descending
				select t), 
			"status" => sortDescending ? (from t in query2
				orderby t.Status descending, t.CreatedDate descending
				select t) : (from t in query2
				orderby t.Status, t.CreatedDate descending
				select t), 
			"title" => sortDescending ? query2.OrderByDescending((TaskItem t) => t.Title) : query2.OrderBy((TaskItem t) => t.Title), 
			_ => query2.OrderByDescending((TaskItem t) => t.CreatedDate), 
		};
		return new PagedResult<TaskSummaryDto>(TotalCount: await query2.LongCountAsync(cancellationToken), Items: await (from t in query2.Skip((r.Page - 1) * r.PageSize).Take(r.PageSize)
			select new TaskSummaryDto(t.Id, t.Title, t.Category, t.Priority, t.Status, t.Progress, t.CreatedDate, t.ScheduledAt, t.StartedAt, t.CompletedAt, t.RetryCount, t.MaxRetries, t.ClaimedByWorkerId, (t.ClaimedByWorker != null) ? t.ClaimedByWorker.Name : null, t.CompanyId, t.QueueName)).ToListAsync(cancellationToken), Page: r.Page, PageSize: r.PageSize);
	}
}
