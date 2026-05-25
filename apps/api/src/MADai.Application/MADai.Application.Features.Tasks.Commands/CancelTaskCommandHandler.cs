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

public class CancelTaskCommandHandler : IRequestHandler<CancelTaskCommand, Unit>
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	private readonly IDateTimeProvider _clock;

	private readonly IEventPublisher _publisher;

	public CancelTaskCommandHandler(IDbContextAccess db, ICurrentUserService currentUser, IDateTimeProvider clock, IEventPublisher publisher)
	{
		_db = db;
		_currentUser = currentUser;
		_clock = clock;
		_publisher = publisher;
	}

	public async Task<Unit> Handle(CancelTaskCommand request, CancellationToken cancellationToken)
	{
		CancelTaskCommand request2 = request;
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
		MADai.Domain.Enums.TaskStatus status = task.Status;
		if ((status == MADai.Domain.Enums.TaskStatus.Completed || status == MADai.Domain.Enums.TaskStatus.Cancelled) ? true : false)
		{
			return Unit.Value;
		}
		task.Status = MADai.Domain.Enums.TaskStatus.Cancelled;
		task.CancelledAt = _clock.UtcNow;
		task.ErrorMessage = request2.Reason ?? "Cancelled by user.";
		await _db.SaveChangesAsync(cancellationToken);
		await _publisher.PublishTaskUpdatedAsync(task.CompanyId, task.Id, new
		{
			Id = task.Id,
			Status = task.Status,
			Event = "Cancelled"
		}, cancellationToken);
		return Unit.Value;
	}
}
