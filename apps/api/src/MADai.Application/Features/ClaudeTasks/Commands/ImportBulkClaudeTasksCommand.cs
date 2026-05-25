using MADai.Application.Abstractions;
using MADai.Application.Features.ClaudeTasks.Queries;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudeTasks.Commands;

public sealed record ImportBulkClaudeTasksCommand(ClaudeBulkImportRequest Request) : IRequest<ClaudeBulkImportResult>;

public class ImportBulkClaudeTasksCommandHandler : IRequestHandler<ImportBulkClaudeTasksCommand, ClaudeBulkImportResult>
{
    private readonly IDbContextAccess _db;
    private readonly IEventPublisher _publisher;

    public ImportBulkClaudeTasksCommandHandler(IDbContextAccess db, IEventPublisher publisher)
    {
        _db = db;
        _publisher = publisher;
    }

    public async Task<ClaudeBulkImportResult> Handle(ImportBulkClaudeTasksCommand command, CancellationToken cancellationToken)
    {
        var items = command.Request.Items ?? Array.Empty<ClaudeBulkImportItem>();
        if (items.Count == 0)
        {
            return new ClaudeBulkImportResult(0, 0, Array.Empty<ClaudeTaskSummaryDto>());
        }

        // Dedupe against currently-active rows (Pending + InProgress). Failed/Completed
        // tasks are intentionally re-queueable once a blocker is resolved.
        var active = new[] { ClaudeTaskStatus.Pending, ClaudeTaskStatus.InProgress };
        var existingTitles = (await _db.ClaudeTasks
            .Where(t => active.Contains(t.Status))
            .Select(t => t.Title)
            .ToListAsync(cancellationToken))
            .Select(s => s.Trim().ToLowerInvariant())
            .ToHashSet();

        var created = new List<ClaudeTask>();
        var skipped = 0;
        var seenInPayload = new HashSet<string>();

        foreach (var i in items)
        {
            if (string.IsNullOrWhiteSpace(i.Title)) { skipped++; continue; }
            var norm = i.Title.Trim().ToLowerInvariant();
            if (norm.Length == 0) { skipped++; continue; }
            if (existingTitles.Contains(norm) || !seenInPayload.Add(norm))
            {
                skipped++;
                continue;
            }

            var entity = new ClaudeTask
            {
                Title = i.Title.Trim(),
                Description = i.Description,
                Notes = i.Notes,
                Priority = i.Priority ?? ClaudeTaskPriority.Normal,
                Status = ClaudeTaskStatus.Pending
            };
            _db.ClaudeTasks.Add(entity);
            created.Add(entity);
        }

        if (created.Count > 0)
        {
            await _db.SaveChangesAsync(cancellationToken);
            foreach (var c in created)
            {
                var dto = new ClaudeTaskSummaryDto(c.Id, c.Title, c.Status, c.Priority, 0, c.CreatedDate, c.ModifiedDate);
                await _publisher.PublishClaudeTaskUpdatedAsync(c.Id,
                    new { type = "created", taskId = c.Id, task = dto }, cancellationToken);
            }
        }

        var summaries = created.Select(c =>
            new ClaudeTaskSummaryDto(c.Id, c.Title, c.Status, c.Priority, 0, c.CreatedDate, c.ModifiedDate))
            .ToList();
        return new ClaudeBulkImportResult(created.Count, skipped, summaries);
    }
}
