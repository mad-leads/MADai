using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Identity;
using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.TaskComments;

public class TaskCommentService : ITaskCommentService
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	private readonly IEventPublisher _publisher;

	public TaskCommentService(IDbContextAccess db, ICurrentUserService currentUser, IEventPublisher publisher)
	{
		_db = db;
		_currentUser = currentUser;
		_publisher = publisher;
	}

	public async Task<IReadOnlyList<TaskCommentDto>> ListAsync(Guid taskId, CancellationToken cancellationToken = default(CancellationToken))
	{
		await EnsureTaskAccessAsync(taskId, cancellationToken);
		return await (from c in _db.TaskComments.AsNoTracking()
			where c.TaskId == taskId
			orderby c.CreatedDate
			join u in _db.Users.AsNoTracking() on c.AuthorUserId equals u.Id
			select new TaskCommentDto(c.Id, c.TaskId, c.AuthorUserId, (u != null) ? u.Email : null, c.Body, c.IsSystem, c.CreatedDate)).ToListAsync(cancellationToken);
	}

	public async Task<TaskCommentDto> AddAsync(Guid taskId, CreateTaskCommentRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (string.IsNullOrWhiteSpace(request.Body))
		{
			throw new AppException("Comment body required.");
		}
		await EnsureTaskAccessAsync(taskId, cancellationToken);
		TaskComment entity = new TaskComment
		{
			TaskId = taskId,
			Body = request.Body.Trim(),
			IsSystem = (request.IsSystem && _currentUser.IsInRole("SystemAdmin")),
			AuthorUserId = _currentUser.UserId
		};
		_db.TaskComments.Add(entity);
		await _db.SaveChangesAsync(cancellationToken);
		TaskCommentDto dto = new TaskCommentDto(entity.Id, entity.TaskId, entity.AuthorUserId, _currentUser.Email, entity.Body, entity.IsSystem, entity.CreatedDate);
		Guid companyId = await (from t in _db.Tasks
			where t.Id == taskId
			select t.CompanyId).FirstOrDefaultAsync(cancellationToken);
		if (companyId != Guid.Empty)
		{
			await _publisher.PublishTaskUpdatedAsync(companyId, taskId, new
			{
				taskId = taskId,
				Event = "Commented",
				commentId = entity.Id
			}, cancellationToken);
		}
		return dto;
	}

	public async Task DeleteAsync(Guid commentId, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaskComment entity = (await _db.TaskComments.FirstOrDefaultAsync((TaskComment c) => c.Id == commentId, cancellationToken)) ?? throw new NotFoundException("TaskComment", commentId);
		if (entity.AuthorUserId != _currentUser.UserId && !_currentUser.IsInRole("SystemAdmin"))
		{
			throw new ForbiddenException("Can only delete your own comments.");
		}
		_db.TaskComments.Remove(entity);
		await _db.SaveChangesAsync(cancellationToken);
	}

	private async Task EnsureTaskAccessAsync(Guid taskId, CancellationToken cancellationToken)
	{
		TaskItem task = (await _db.Tasks.AsNoTracking().FirstOrDefaultAsync((TaskItem t) => t.Id == taskId, cancellationToken)) ?? throw new NotFoundException("Task", taskId);
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			if (task.CompanyId != cid)
			{
				throw new ForbiddenException();
			}
		}
	}
}
