using System;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using MediatR;

namespace MADai.Application.Features.ClaudeTasks.Commands;

public class CreateClaudeTaskCommandHandler : IRequestHandler<CreateClaudeTaskCommand, ClaudeTaskDetailDto>
{
	private readonly IDbContextAccess _db;

	private readonly IEventPublisher _publisher;

	public CreateClaudeTaskCommandHandler(IDbContextAccess db, IEventPublisher publisher)
	{
		_db = db;
		_publisher = publisher;
	}

	public async Task<ClaudeTaskDetailDto> Handle(CreateClaudeTaskCommand command, CancellationToken cancellationToken)
	{
		CreateClaudeTaskRequest r = command.Request;
		ClaudeTask entity = new ClaudeTask
		{
			Title = r.Title.Trim(),
			Description = r.Description,
			Notes = r.Notes,
			Status = ClaudeTaskStatus.Pending,
			Priority = r.Priority.GetValueOrDefault(ClaudeTaskPriority.Normal)
		};
		_db.ClaudeTasks.Add(entity);
		await _db.SaveChangesAsync(cancellationToken);
		ClaudeTaskDetailDto dto = new ClaudeTaskDetailDto(entity.Id, entity.Title, entity.Description, entity.Notes, entity.Status, entity.Priority, Array.Empty<ClaudeTaskAttachmentDto>(), entity.CreatedDate, entity.ModifiedDate);
		await _publisher.PublishClaudeTaskUpdatedAsync(entity.Id, new
		{
			type = "created",
			taskId = entity.Id,
			task = dto
		}, cancellationToken);
		return dto;
	}
}
