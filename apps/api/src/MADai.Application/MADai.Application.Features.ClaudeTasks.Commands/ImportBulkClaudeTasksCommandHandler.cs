using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudeTasks.Commands;

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
		IReadOnlyList<ClaudeBulkImportItem> items = command.Request.Items ?? Array.Empty<ClaudeBulkImportItem>();
		if (items.Count == 0)
		{
			return new ClaudeBulkImportResult(0, 0, Array.Empty<ClaudeTaskSummaryDto>());
		}
		ClaudeTaskStatus[] active = new ClaudeTaskStatus[2]
		{
			ClaudeTaskStatus.Pending,
			ClaudeTaskStatus.InProgress
		};
		HashSet<string> existingTitles = (await (from t in _db.ClaudeTasks
			where active.Contains(t.Status)
			select t.Title).ToListAsync(cancellationToken)).Select((string s) => s.Trim().ToLowerInvariant()).ToHashSet();
		List<ClaudeTask> created = new List<ClaudeTask>();
		int skipped = 0;
		HashSet<string> seenInPayload = new HashSet<string>();
		foreach (ClaudeBulkImportItem i in items)
		{
			if (string.IsNullOrWhiteSpace(i.Title))
			{
				skipped++;
				continue;
			}
			string norm = i.Title.Trim().ToLowerInvariant();
			if (norm.Length == 0)
			{
				skipped++;
				continue;
			}
			if (existingTitles.Contains(norm) || !seenInPayload.Add(norm))
			{
				skipped++;
				continue;
			}
			ClaudeTask entity = new ClaudeTask
			{
				Title = i.Title.Trim(),
				Description = i.Description,
				Notes = i.Notes,
				Priority = i.Priority.GetValueOrDefault(ClaudeTaskPriority.Normal),
				Status = ClaudeTaskStatus.Pending
			};
			_db.ClaudeTasks.Add(entity);
			created.Add(entity);
		}
		if (created.Count > 0)
		{
			await _db.SaveChangesAsync(cancellationToken);
			foreach (ClaudeTask c2 in created)
			{
				ClaudeTaskSummaryDto dto = new ClaudeTaskSummaryDto(c2.Id, c2.Title, c2.Status, c2.Priority, 0, c2.CreatedDate, c2.ModifiedDate);
				await _publisher.PublishClaudeTaskUpdatedAsync(c2.Id, new
				{
					type = "created",
					taskId = c2.Id,
					task = dto
				}, cancellationToken);
			}
		}
		List<ClaudeTaskSummaryDto> summaries = created.Select((ClaudeTask c) => new ClaudeTaskSummaryDto(c.Id, c.Title, c.Status, c.Priority, 0, c.CreatedDate, c.ModifiedDate)).ToList();
		return new ClaudeBulkImportResult(created.Count, skipped, summaries);
	}
}
