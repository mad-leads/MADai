using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Application.Features.ClaudeTasks.Queries;
using MADai.Application.Features.ClaudeTasks.StateMachine;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudeTasks.Commands;

public sealed record UpdateClaudeTaskCommand(Guid Id, UpdateClaudeTaskRequest Request, bool Override) : IRequest<ClaudeTaskDetailDto>;

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
        var task = await _db.ClaudeTasks.FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken)
            ?? throw new NotFoundException("ClaudeTask", command.Id);

        var r = command.Request;

        // Status transition gate - run BEFORE applying the change so we know the actual move.
        if (r.Status is { } target && target != task.Status)
        {
            if (ClaudeTaskTransitions.IsTerminal(task.Status) && !command.Override)
            {
                throw new ConflictException(
                    $"Task is in terminal status {task.Status}. Pass override=true to force a change.");
            }
            if (!ClaudeTaskTransitions.IsAllowed(task.Status, target))
            {
                throw new ConflictException(
                    $"Illegal status transition: {task.Status} -> {target}.");
            }
            task.Status = target;
        }

        // Partial - only apply non-null fields.
        if (r.Title is not null && r.Title.Length > 0) task.Title = r.Title.Trim();
        if (r.Description is not null) task.Description = r.Description;
        if (r.Notes is not null) task.Notes = r.Notes;
        if (r.Priority is { } p) task.Priority = p;

        await _db.SaveChangesAsync(cancellationToken);

        var dto = new ClaudeTaskDetailDto(
            task.Id, task.Title, task.Description, task.Notes,
            task.Status, task.Priority,
            ClaudeTaskAttachmentsParser.Parse(task.AttachmentsJson),
            task.CreatedDate, task.ModifiedDate);

        await _publisher.PublishClaudeTaskUpdatedAsync(task.Id, new { type = "updated", taskId = task.Id, task = dto }, cancellationToken);
        return dto;
    }
}
