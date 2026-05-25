using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record ClaudeTaskQueryRequest : PageQuery
{
	public ClaudeTaskStatus[]? Statuses { get; init; }

	public ClaudeTaskPriority? MinPriority { get; init; }

	public bool? IncludeTerminal { get; init; }

	public new ClaudeTaskQueryRequest Normalize(int maxPageSize = 200)
	{
		ClaudeTaskQueryRequest obj = this with
		{
			Page = ((base.Page < 1) ? 1 : base.Page)
		};
		int pageSize = ((base.PageSize < 1) ? 25 : ((base.PageSize <= maxPageSize) ? base.PageSize : maxPageSize));
		obj.PageSize = pageSize;
		return obj;
	}
}
