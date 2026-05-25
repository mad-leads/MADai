using System.Text.Json;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudeTasks.Queries;

public sealed record GetClaudeTaskByIdQuery(Guid Id) : IRequest<ClaudeTaskDetailDto>;

public class GetClaudeTaskByIdQueryHandler : IRequestHandler<GetClaudeTaskByIdQuery, ClaudeTaskDetailDto>
{
    private readonly IDbContextAccess _db;
    public GetClaudeTaskByIdQueryHandler(IDbContextAccess db) => _db = db;

    public async Task<ClaudeTaskDetailDto> Handle(GetClaudeTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _db.ClaudeTasks
            .AsNoTracking()
            .Where(t => t.Id == request.Id)
            .Select(t => new
            {
                t.Id, t.Title, t.Description, t.Notes,
                t.Status, t.Priority, t.AttachmentsJson,
                t.CreatedDate, t.ModifiedDate
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("ClaudeTask", request.Id);

        return new ClaudeTaskDetailDto(
            task.Id, task.Title, task.Description, task.Notes,
            task.Status, task.Priority,
            ClaudeTaskAttachmentsParser.Parse(task.AttachmentsJson),
            task.CreatedDate, task.ModifiedDate);
    }
}

internal static class ClaudeTaskAttachmentsParser
{
    public static IReadOnlyList<ClaudeTaskAttachmentDto> Parse(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return Array.Empty<ClaudeTaskAttachmentDto>();
        try
        {
            return JsonSerializer.Deserialize<List<ClaudeTaskAttachmentDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<ClaudeTaskAttachmentDto>();
        }
        catch { return Array.Empty<ClaudeTaskAttachmentDto>(); }
    }

    public static string Serialize(IEnumerable<ClaudeTaskAttachmentDto> items) =>
        JsonSerializer.Serialize(items, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
}
