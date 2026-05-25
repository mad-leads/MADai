using System.Text.Json;
using MADai.Application.Abstractions;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudeTasks.Queries;

public sealed record GetClaudeTasksQuery(ClaudeTaskQueryRequest Request) : IRequest<PagedResult<ClaudeTaskSummaryDto>>;

public class GetClaudeTasksQueryHandler : IRequestHandler<GetClaudeTasksQuery, PagedResult<ClaudeTaskSummaryDto>>
{
    private readonly IDbContextAccess _db;
    public GetClaudeTasksQueryHandler(IDbContextAccess db) => _db = db;

    public async Task<PagedResult<ClaudeTaskSummaryDto>> Handle(GetClaudeTasksQuery request, CancellationToken cancellationToken)
    {
        var r = request.Request.Normalize();
        var query = _db.ClaudeTasks.AsNoTracking().AsQueryable();

        if (r.Statuses is { Length: > 0 } statuses)
        {
            query = query.Where(t => statuses.Contains(t.Status));
        }
        else if (r.IncludeTerminal != true)
        {
            // Default: hide Completed / Cancelled / Failed so the page doesn't drown in done rows.
            var active = new[]
            {
                ClaudeTaskStatus.Pending,
                ClaudeTaskStatus.InProgress,
                ClaudeTaskStatus.ToBeDeployed,
                ClaudeTaskStatus.Deferred
            };
            query = query.Where(t => active.Contains(t.Status));
        }

        if (r.MinPriority is { } minP)
        {
            // Priority enum: lower number = higher importance. So "MinPriority=Normal" means
            // include Normal, High, Critical -> priority <= Normal.
            query = query.Where(t => t.Priority <= minP);
        }

        if (!string.IsNullOrWhiteSpace(r.Search))
        {
            var term = r.Search.Trim();
            query = query.Where(t => EF.Functions.Like(t.Title, $"%{term}%")
                                  || (t.Description != null && EF.Functions.Like(t.Description, $"%{term}%")));
        }

        // Bucketing: active first (priority asc, created asc), then ToBeDeployed, then terminal (created desc).
        // We sort flat with a CASE expression so SQL Server can use the (Status, Priority, Id) index.
        var sorted = query
            .OrderBy(t => t.Status == ClaudeTaskStatus.Pending
                       || t.Status == ClaudeTaskStatus.InProgress
                       || t.Status == ClaudeTaskStatus.Deferred  ? 0
                       : t.Status == ClaudeTaskStatus.ToBeDeployed ? 1 : 2)
            .ThenBy(t => t.Priority)
            .ThenByDescending(t => t.Status == ClaudeTaskStatus.Completed
                                || t.Status == ClaudeTaskStatus.Cancelled
                                || t.Status == ClaudeTaskStatus.Failed
                                ? t.CreatedDate : DateTime.MinValue)
            .ThenBy(t => t.CreatedDate);

        var total = await sorted.LongCountAsync(cancellationToken);
        var page = await sorted
            .Skip((r.Page - 1) * r.PageSize)
            .Take(r.PageSize)
            .Select(t => new
            {
                t.Id,
                t.Title,
                t.Status,
                t.Priority,
                t.AttachmentsJson,
                t.CreatedDate,
                t.ModifiedDate
            })
            .ToListAsync(cancellationToken);

        var items = page.Select(t => new ClaudeTaskSummaryDto(
            t.Id,
            t.Title,
            t.Status,
            t.Priority,
            CountAttachments(t.AttachmentsJson),
            t.CreatedDate,
            t.ModifiedDate)).ToList();

        return new PagedResult<ClaudeTaskSummaryDto>(items, r.Page, r.PageSize, total);
    }

    private static int CountAttachments(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return 0;
        try
        {
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.ValueKind == JsonValueKind.Array ? doc.RootElement.GetArrayLength() : 0;
        }
        catch { return 0; }
    }
}
