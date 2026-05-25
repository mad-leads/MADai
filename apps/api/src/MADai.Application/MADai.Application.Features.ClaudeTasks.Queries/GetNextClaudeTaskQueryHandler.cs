using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudeTasks.Queries;

public class GetNextClaudeTaskQueryHandler : IRequestHandler<GetNextClaudeTaskQuery, ClaudeTaskDetailDto?>
{
	private readonly IDbContextAccess _db;

	public GetNextClaudeTaskQueryHandler(IDbContextAccess db)
	{
		_db = db;
	}

	public async Task<ClaudeTaskDetailDto?> Handle(GetNextClaudeTaskQuery request, CancellationToken cancellationToken)
	{
		ClaudeTaskStatus[] active = new ClaudeTaskStatus[3]
		{
			ClaudeTaskStatus.Pending,
			ClaudeTaskStatus.InProgress,
			ClaudeTaskStatus.Deferred
		};
		var t = await (from x in _db.ClaudeTasks.AsNoTracking()
			where active.Contains(x.Status)
			orderby x.Priority, x.CreatedDate
			select new { x.Id, x.Title, x.Description, x.Notes, x.Status, x.Priority, x.AttachmentsJson, x.CreatedDate, x.ModifiedDate }).FirstOrDefaultAsync(cancellationToken);
		if (t == null)
		{
			return null;
		}
		return new ClaudeTaskDetailDto(t.Id, t.Title, t.Description, t.Notes, t.Status, t.Priority, ClaudeTaskAttachmentsParser.Parse(t.AttachmentsJson), t.CreatedDate, t.ModifiedDate);
	}
}
