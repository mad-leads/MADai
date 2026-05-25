using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudeTasks.Queries;

public class GetClaudeTasksQueryHandler : IRequestHandler<GetClaudeTasksQuery, PagedResult<ClaudeTaskSummaryDto>>
{
	private readonly IDbContextAccess _db;

	public GetClaudeTasksQueryHandler(IDbContextAccess db)
	{
		_db = db;
	}

	public async Task<PagedResult<ClaudeTaskSummaryDto>> Handle(GetClaudeTasksQuery request, CancellationToken cancellationToken)
	{
		ClaudeTaskQueryRequest r = request.Request.Normalize();
		IQueryable<ClaudeTask> query = _db.ClaudeTasks.AsNoTracking().AsQueryable();
		ClaudeTaskStatus[] statuses = r.Statuses;
		if (statuses != null && statuses.Length > 0)
		{
			query = query.Where((ClaudeTask t) => statuses.Contains(t.Status));
		}
		else if (!r.IncludeTerminal.GetValueOrDefault())
		{
			ClaudeTaskStatus[] active = new ClaudeTaskStatus[4]
			{
				ClaudeTaskStatus.Pending,
				ClaudeTaskStatus.InProgress,
				ClaudeTaskStatus.ToBeDeployed,
				ClaudeTaskStatus.Deferred
			};
			query = query.Where((ClaudeTask t) => active.Contains(t.Status));
		}
		ClaudeTaskPriority? minPriority = r.MinPriority;
		if (minPriority.HasValue)
		{
			ClaudeTaskPriority minP = minPriority.GetValueOrDefault();
			query = query.Where((ClaudeTask t) => (int)t.Priority <= (int)minP);
		}
		if (!string.IsNullOrWhiteSpace(r.Search))
		{
			string term = r.Search.Trim();
			query = query.Where((ClaudeTask t) => EF.Functions.Like(t.Title, $"%{term}%") || (t.Description != null && EF.Functions.Like(t.Description, $"%{term}%")));
		}
		IOrderedQueryable<ClaudeTask> sorted = from t in query
			orderby ((int)t.Status == 0 || (int)t.Status == 10 || (int)t.Status == 60) ? 0 : (((int)t.Status == 20) ? 1 : 2), t.Priority, ((int)t.Status == 30 || (int)t.Status == 50 || (int)t.Status == 40) ? t.CreatedDate : DateTime.MinValue descending, t.CreatedDate
			select t;
		return new PagedResult<ClaudeTaskSummaryDto>(TotalCount: await sorted.LongCountAsync(cancellationToken), Items: (await (from t in sorted.Skip((r.Page - 1) * r.PageSize).Take(r.PageSize)
			select new { t.Id, t.Title, t.Status, t.Priority, t.AttachmentsJson, t.CreatedDate, t.ModifiedDate }).ToListAsync(cancellationToken)).Select(t => new ClaudeTaskSummaryDto(t.Id, t.Title, t.Status, t.Priority, CountAttachments(t.AttachmentsJson), t.CreatedDate, t.ModifiedDate)).ToList(), Page: r.Page, PageSize: r.PageSize);
	}

	private static int CountAttachments(string? json)
	{
		if (string.IsNullOrWhiteSpace(json))
		{
			return 0;
		}
		try
		{
			using JsonDocument doc = JsonDocument.Parse(json);
			return (doc.RootElement.ValueKind == JsonValueKind.Array) ? doc.RootElement.GetArrayLength() : 0;
		}
		catch
		{
			return 0;
		}
	}
}
