using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudeTasks.Commands;

public class DeleteClaudeTaskCommandHandler : IRequestHandler<DeleteClaudeTaskCommand, Unit>
{
	private readonly IDbContextAccess _db;

	private readonly IEventPublisher _publisher;

	public DeleteClaudeTaskCommandHandler(IDbContextAccess db, IEventPublisher publisher)
	{
		_db = db;
		_publisher = publisher;
	}

	public async Task<Unit> Handle(DeleteClaudeTaskCommand request, CancellationToken cancellationToken)
	{
		DeleteClaudeTaskCommand request2 = request;
		ClaudeTask task = (await _db.ClaudeTasks.FirstOrDefaultAsync((ClaudeTask t) => t.Id == request2.Id, cancellationToken)) ?? throw new NotFoundException("ClaudeTask", request2.Id);
		_db.ClaudeTasks.Remove(task);
		await _db.SaveChangesAsync(cancellationToken);
		await _publisher.PublishClaudeTaskUpdatedAsync(task.Id, new
		{
			type = "deleted",
			taskId = task.Id
		}, cancellationToken);
		return Unit.Value;
	}
}
