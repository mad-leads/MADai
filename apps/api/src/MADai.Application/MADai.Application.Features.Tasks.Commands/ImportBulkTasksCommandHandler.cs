using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Tasks.Commands;

public class ImportBulkTasksCommandHandler : IRequestHandler<ImportBulkTasksCommand, ImportBulkTasksResult>
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	private readonly IEventPublisher _publisher;

	private readonly IDateTimeProvider _clock;

	public ImportBulkTasksCommandHandler(IDbContextAccess db, ICurrentUserService currentUser, IEventPublisher publisher, IDateTimeProvider clock)
	{
		_db = db;
		_currentUser = currentUser;
		_publisher = publisher;
		_clock = clock;
	}

	public async Task<ImportBulkTasksResult> Handle(ImportBulkTasksCommand command, CancellationToken cancellationToken)
	{
		Guid companyId = _currentUser.CompanyId ?? throw new ForbiddenException("Company context required.");
		if (command.Items == null || command.Items.Count == 0)
		{
			return new ImportBulkTasksResult(0, 0, Array.Empty<Guid>());
		}
		MADai.Domain.Enums.TaskStatus[] active = new MADai.Domain.Enums.TaskStatus[4]
		{
			MADai.Domain.Enums.TaskStatus.Queued,
			MADai.Domain.Enums.TaskStatus.Scheduled,
			MADai.Domain.Enums.TaskStatus.Running,
			MADai.Domain.Enums.TaskStatus.Claimed
		};
		HashSet<string> existingTitles = (await (from t in _db.Tasks
			where t.CompanyId == companyId && active.Contains(t.Status)
			select t.Title).ToListAsync(cancellationToken)).Select((string s) => s.Trim().ToLowerInvariant()).ToHashSet();
		List<TaskItem> created = new List<TaskItem>();
		HashSet<string> seenInPayload = new HashSet<string>();
		int skipped = 0;
		foreach (TaskBulkImportItem i in command.Items)
		{
			if (string.IsNullOrWhiteSpace(i.Title))
			{
				skipped++;
				continue;
			}
			string norm = i.Title.Trim().ToLowerInvariant();
			if (existingTitles.Contains(norm) || !seenInPayload.Add(norm))
			{
				skipped++;
				continue;
			}
			TaskItem entity = new TaskItem
			{
				CompanyId = companyId,
				Title = i.Title.Trim(),
				Description = i.Description,
				Category = i.Category,
				Priority = i.Priority.GetValueOrDefault(TaskPriority.Normal),
				Status = MADai.Domain.Enums.TaskStatus.Queued,
				Origin = TaskOrigin.Api,
				QueueName = (i.Queue ?? "default"),
				PromptPayload = i.PromptPayload,
				MaxRetries = 3
			};
			_db.Tasks.Add(entity);
			created.Add(entity);
		}
		if (created.Count > 0)
		{
			await _db.SaveChangesAsync(cancellationToken);
			foreach (TaskItem c2 in created)
			{
				await _publisher.PublishTaskUpdatedAsync(c2.CompanyId, c2.Id, new
				{
					Id = c2.Id,
					Status = c2.Status,
					Event = "Created"
				}, cancellationToken);
			}
		}
		return new ImportBulkTasksResult(created.Count, skipped, created.Select((TaskItem c) => c.Id).ToList());
	}
}
