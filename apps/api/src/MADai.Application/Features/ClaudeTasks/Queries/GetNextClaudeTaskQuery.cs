using MADai.Application.Abstractions;
using MADai.Domain.Enums;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudeTasks.Queries;

public sealed record GetNextClaudeTaskQuery : IRequest<ClaudeTaskDetailDto?>;

public class GetNextClaudeTaskQueryHandler : IRequestHandler<GetNextClaudeTaskQuery, ClaudeTaskDetailDto?>
{
    private readonly IDbContextAccess _db;
    public GetNextClaudeTaskQueryHandler(IDbContextAccess db) => _db = db;

    public async Task<ClaudeTaskDetailDto?> Handle(GetNextClaudeTaskQuery request, CancellationToken cancellationToken)
    {
        // Active statuses only - Failed/Completed/Cancelled are terminal and must never appear here.
        var active = new[] { ClaudeTaskStatus.Pending, ClaudeTaskStatus.InProgress, ClaudeTaskStatus.Deferred };

        var t = await _db.ClaudeTasks
            .AsNoTracking()
            .Where(x => active.Contains(x.Status))
            .OrderBy(x => x.Priority)            // 1=Critical first
            .ThenBy(x => x.CreatedDate)          // FIFO within priority
            .Select(x => new
            {
                x.Id, x.Title, x.Description, x.Notes,
                x.Status, x.Priority, x.AttachmentsJson,
                x.CreatedDate, x.ModifiedDate
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (t is null) return null;
        return new ClaudeTaskDetailDto(
            t.Id, t.Title, t.Description, t.Notes,
            t.Status, t.Priority,
            ClaudeTaskAttachmentsParser.Parse(t.AttachmentsJson),
            t.CreatedDate, t.ModifiedDate);
    }
}
