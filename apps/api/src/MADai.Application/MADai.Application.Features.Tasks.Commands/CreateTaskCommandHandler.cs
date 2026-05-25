using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Application.Features.Webhooks;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Tasks.Commands;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDetailDto>
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	private readonly IEventPublisher _publisher;

	private readonly IWebhookPublisher _webhooks;

	private readonly IDateTimeProvider _clock;

	private readonly IMapper _mapper;

	public CreateTaskCommandHandler(IDbContextAccess db, ICurrentUserService currentUser, IEventPublisher publisher, IWebhookPublisher webhooks, IDateTimeProvider clock, IMapper mapper)
	{
		_db = db;
		_currentUser = currentUser;
		_publisher = publisher;
		_webhooks = webhooks;
		_clock = clock;
		_mapper = mapper;
	}

	public async Task<TaskDetailDto> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
	{
		CreateTaskRequest req = command.Request;
		Guid companyId = _currentUser.CompanyId ?? throw new ForbiddenException("Company context required to create tasks.");
		TaskItem task = new TaskItem
		{
			Title = req.Title.Trim(),
			Description = req.Description,
			Category = req.Category,
			Priority = req.Priority,
			Origin = TaskOrigin.User,
			QueueName = (req.Queue ?? "default"),
			PromptPayload = req.PromptPayload,
			InputPayload = req.InputPayload,
			TimeoutSeconds = req.TimeoutSeconds.GetValueOrDefault(3600),
			MaxRetries = req.MaxRetries.GetValueOrDefault(3),
			ScheduledAt = req.ScheduledAt,
			ParentTaskId = req.ParentTaskId,
			TemplateId = req.TemplateId,
			IsRecurring = req.IsRecurring,
			CronExpression = req.CronExpression,
			CompanyId = companyId,
			Status = ((req.ScheduledAt.HasValue && req.ScheduledAt.Value > _clock.UtcNow) ? MADai.Domain.Enums.TaskStatus.Scheduled : MADai.Domain.Enums.TaskStatus.Queued)
		};
		IReadOnlyList<Guid> dependencies = req.DependsOnTaskIds;
		if (dependencies != null && dependencies.Count > 0)
		{
			foreach (Guid dep in dependencies.Distinct())
			{
				task.Dependencies.Add(new TaskDependency
				{
					TaskId = task.Id,
					DependsOnTaskId = dep
				});
			}
			if (task.Status == MADai.Domain.Enums.TaskStatus.Queued)
			{
				task.Status = MADai.Domain.Enums.TaskStatus.Scheduled;
			}
		}
		IReadOnlyList<string> tags = req.Tags;
		if (tags != null && tags.Count > 0)
		{
			List<string> normalized = (from t in tags
				select t.Trim() into t
				where !string.IsNullOrEmpty(t)
				select t).Distinct().ToList();
			List<TaskTag> existingTags = await _db.TaskTags.Where((TaskTag tag) => tag.CompanyId == companyId && normalized.Contains(tag.Name)).ToListAsync(cancellationToken);
			foreach (string name in normalized)
			{
				TaskTag tag2 = existingTags.FirstOrDefault((TaskTag t) => t.Name == name);
				if (tag2 == null)
				{
					tag2 = new TaskTag
					{
						Name = name,
						CompanyId = companyId
					};
					_db.TaskTags.Add(tag2);
				}
				task.TagLinks.Add(new TaskTagLink
				{
					TaskId = task.Id,
					TagId = tag2.Id,
					Tag = tag2
				});
			}
		}
		_db.Tasks.Add(task);
		await _db.SaveChangesAsync(cancellationToken);
		TaskDetailDto dto = _mapper.Map<TaskDetailDto>(task);
		await _publisher.PublishTaskUpdatedAsync(companyId, task.Id, new
		{
			Id = task.Id,
			Status = task.Status,
			Event = "Created"
		}, cancellationToken);
		await _webhooks.PublishAsync(companyId, "task.created", new { task.Id, task.Title, task.Status, task.Priority, task.Category, task.QueueName }, cancellationToken);
		return dto;
	}
}
