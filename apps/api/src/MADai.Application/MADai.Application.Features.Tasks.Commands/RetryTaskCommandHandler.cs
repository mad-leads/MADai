using System;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Tasks.Commands;

public class RetryTaskCommandHandler : IRequestHandler<RetryTaskCommand, Unit>
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	private readonly IDateTimeProvider _clock;

	private readonly IEventPublisher _publisher;

	public RetryTaskCommandHandler(IDbContextAccess db, ICurrentUserService currentUser, IDateTimeProvider clock, IEventPublisher publisher)
	{
		_db = db;
		_currentUser = currentUser;
		_clock = clock;
		_publisher = publisher;
	}

	public async Task<Unit> Handle(RetryTaskCommand request, CancellationToken cancellationToken)
	{
		RetryTaskCommand request2 = request;
		TaskItem task = (await _db.Tasks.FirstOrDefaultAsync((TaskItem t) => t.Id == request2.TaskId, cancellationToken)) ?? throw new NotFoundException("Task", request2.TaskId);
		Guid? companyId2 = _currentUser.CompanyId;
		if (companyId2.HasValue)
		{
			Guid companyId = companyId2.GetValueOrDefault();
			if (task.CompanyId != companyId)
			{
				throw new ForbiddenException();
			}
		}
		if (task.IsDeadLetter && !request2.ForceFromDeadLetter)
		{
			throw new ConflictException("Task is in the dead-letter queue. Pass ForceFromDeadLetter=true to retry.");
		}
		_db.TaskRetries.Add(new TaskRetry
		{
			TaskId = task.Id,
			AttemptNumber = task.RetryCount + 1,
			AttemptedAt = _clock.UtcNow,
			FailureReason = task.ErrorMessage,
			Strategy = "Manual"
		});
		task.RetryCount++;
		task.Status = MADai.Domain.Enums.TaskStatus.Queued;
		task.ErrorMessage = null;
		task.ErrorStack = null;
		task.IsDeadLetter = false;
		task.NextRetryAt = null;
		task.ClaimedAt = null;
		task.ClaimedByWorkerId = null;
		task.ClaimToken = null;
		task.StartedAt = null;
		task.CompletedAt = null;
		task.Progress = 0;
		await _db.SaveChangesAsync(cancellationToken);
		await _publisher.PublishTaskUpdatedAsync(task.CompanyId, task.Id, new
		{
			Id = task.Id,
			Status = task.Status,
			Event = "Retried"
		}, cancellationToken);
		return Unit.Value;
	}
}
