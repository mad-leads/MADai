using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Application.Features.ClaudeTasks.Queries;
using MADai.Application.Features.ClaudeTasks.StateMachine;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudeTasks.Commands;

public class UpdateClaudeTaskCommandHandler : IRequestHandler<UpdateClaudeTaskCommand, ClaudeTaskDetailDto>
{
	private readonly IDbContextAccess _db;

	private readonly IEventPublisher _publisher;

	public UpdateClaudeTaskCommandHandler(IDbContextAccess db, IEventPublisher publisher)
	{
		_db = db;
		_publisher = publisher;
	}

	public async Task<ClaudeTaskDetailDto> Handle(UpdateClaudeTaskCommand command, CancellationToken cancellationToken)
	{
		UpdateClaudeTaskCommand command2 = command;
		ClaudeTask task = (await _db.ClaudeTasks.FirstOrDefaultAsync((ClaudeTask t) => t.Id == command2.Id, cancellationToken)) ?? throw new NotFoundException("ClaudeTask", command2.Id);
		UpdateClaudeTaskRequest r = command2.Request;
		ClaudeTaskStatus? status = r.Status;
		if (status.HasValue)
		{
			ClaudeTaskStatus target = status.GetValueOrDefault();
			if (target != task.Status)
			{
				if (ClaudeTaskTransitions.IsTerminal(task.Status) && !command2.Override)
				{
					throw new ConflictException($"Task is in terminal status {task.Status}. Pass override=true to force a change.");
				}
				if (!ClaudeTaskTransitions.IsAllowed(task.Status, target))
				{
					throw new ConflictException($"Illegal status transition: {task.Status} -> {target}.");
				}
				task.Status = target;
			}
		}
		if (r.Title != null && r.Title.Length > 0)
		{
			task.Title = r.Title.Trim();
		}
		if (r.Description != null)
		{
			task.Description = r.Description;
		}
		if (r.Notes != null)
		{
			task.Notes = r.Notes;
		}
		ClaudeTaskPriority? priority = r.Priority;
		if (priority.HasValue)
		{
			ClaudeTaskPriority p = priority.GetValueOrDefault();
			task.Priority = p;
		}
		await _db.SaveChangesAsync(cancellationToken);
		ClaudeTaskDetailDto dto = new ClaudeTaskDetailDto(task.Id, task.Title, task.Description, task.Notes, task.Status, task.Priority, ClaudeTaskAttachmentsParser.Parse(task.AttachmentsJson), task.CreatedDate, task.ModifiedDate);
		await _publisher.PublishClaudeTaskUpdatedAsync(task.Id, new
		{
			type = "updated",
			taskId = task.Id,
			task = dto
		}, cancellationToken);
		return dto;
	}
}
