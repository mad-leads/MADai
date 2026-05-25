using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudeTasks.Queries;

public class GetClaudeTaskByIdQueryHandler : IRequestHandler<GetClaudeTaskByIdQuery, ClaudeTaskDetailDto>
{
	private readonly IDbContextAccess _db;

	public GetClaudeTaskByIdQueryHandler(IDbContextAccess db)
	{
		_db = db;
	}

	public async Task<ClaudeTaskDetailDto> Handle(GetClaudeTaskByIdQuery request, CancellationToken cancellationToken)
	{
		GetClaudeTaskByIdQuery request2 = request;
		var task = (await (from t in _db.ClaudeTasks.AsNoTracking()
			where t.Id == request2.Id
			select new { t.Id, t.Title, t.Description, t.Notes, t.Status, t.Priority, t.AttachmentsJson, t.CreatedDate, t.ModifiedDate }).FirstOrDefaultAsync(cancellationToken)) ?? throw new NotFoundException("ClaudeTask", request2.Id);
		return new ClaudeTaskDetailDto(task.Id, task.Title, task.Description, task.Notes, task.Status, task.Priority, ClaudeTaskAttachmentsParser.Parse(task.AttachmentsJson), task.CreatedDate, task.ModifiedDate);
	}
}
