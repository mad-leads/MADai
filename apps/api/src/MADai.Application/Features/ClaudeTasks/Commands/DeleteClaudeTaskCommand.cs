using MADai.Application.Abstractions;
using MADai.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudeTasks.Commands;

public sealed record DeleteClaudeTaskCommand(Guid Id) : IRequest<Unit>;

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
        var task = await _db.ClaudeTasks.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("ClaudeTask", request.Id);

        _db.ClaudeTasks.Remove(task); // AuditingInterceptor converts this to soft-delete via IsDeleted
        await _db.SaveChangesAsync(cancellationToken);
        await _publisher.PublishClaudeTaskUpdatedAsync(task.Id, new { type = "deleted", taskId = task.Id }, cancellationToken);
        return Unit.Value;
    }
}
