using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Tasks.Commands;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDetailDto>
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	private readonly IMapper _mapper;

	private readonly IEventPublisher _publisher;

	public UpdateTaskCommandHandler(IDbContextAccess db, ICurrentUserService currentUser, IMapper mapper, IEventPublisher publisher)
	{
		_db = db;
		_currentUser = currentUser;
		_mapper = mapper;
		_publisher = publisher;
	}

	public async Task<TaskDetailDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
	{
		UpdateTaskCommand request2 = request;
		TaskItem task = (await _db.Tasks.Include((TaskItem t) => t.ClaimedByWorker).Include((TaskItem t) => t.Artifacts).Include((TaskItem t) => t.Dependencies)
			.ThenInclude((TaskDependency d) => d.DependsOnTask)
			.Include((TaskItem t) => t.TagLinks)
			.ThenInclude((TaskTagLink l) => l.Tag)
			.FirstOrDefaultAsync((TaskItem t) => t.Id == request2.TaskId, cancellationToken)) ?? throw new NotFoundException("Task", request2.TaskId);
		Guid? companyId2 = _currentUser.CompanyId;
		if (companyId2.HasValue)
		{
			Guid companyId = companyId2.GetValueOrDefault();
			if (task.CompanyId != companyId)
			{
				throw new ForbiddenException();
			}
		}
		MADai.Domain.Enums.TaskStatus status = task.Status;
		if ((status == MADai.Domain.Enums.TaskStatus.Running || status == MADai.Domain.Enums.TaskStatus.Completed || status == MADai.Domain.Enums.TaskStatus.Cancelled) ? true : false)
		{
			throw new ConflictException($"Cannot update task in status {task.Status}.");
		}
		UpdateTaskRequest req = request2.Request;
		if (!string.IsNullOrWhiteSpace(req.Title))
		{
			task.Title = req.Title.Trim();
		}
		if (req.Description != null)
		{
			task.Description = req.Description;
		}
		TaskPriority? priority = req.Priority;
		if (priority.HasValue)
		{
			TaskPriority p = priority.GetValueOrDefault();
			task.Priority = p;
		}
		if (req.Queue != null)
		{
			task.QueueName = req.Queue;
		}
		DateTime? scheduledAt = req.ScheduledAt;
		if (scheduledAt.HasValue)
		{
			DateTime s = scheduledAt.GetValueOrDefault();
			task.ScheduledAt = s;
		}
		int? maxRetries = req.MaxRetries;
		if (maxRetries.HasValue)
		{
			int mr = maxRetries.GetValueOrDefault();
			task.MaxRetries = mr;
		}
		maxRetries = req.TimeoutSeconds;
		if (maxRetries.HasValue)
		{
			int ts = maxRetries.GetValueOrDefault();
			task.TimeoutSeconds = ts;
		}
		await _db.SaveChangesAsync(cancellationToken);
		await _publisher.PublishTaskUpdatedAsync(task.CompanyId, task.Id, new
		{
			Id = task.Id,
			Status = task.Status,
			Event = "Updated"
		}, cancellationToken);
		return _mapper.Map<TaskDetailDto>(task);
	}
}
