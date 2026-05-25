using System;
using MADai.Domain.Enums;

namespace MADai.Shared.Contracts;

public sealed record TaskQueryRequest : PageQuery
{
	public MADai.Domain.Enums.TaskStatus[]? Statuses { get; init; }

	public TaskCategory[]? Categories { get; init; }

	public TaskPriority? MinPriority { get; init; }

	public Guid? ClaimedByWorkerId { get; init; }

	public DateTime? CreatedFrom { get; init; }

	public DateTime? CreatedTo { get; init; }

	public Guid? ParentTaskId { get; init; }

	public string? Queue { get; init; }

	public bool? IsDeadLetter { get; init; }

	public new TaskQueryRequest Normalize(int maxPageSize = 200) => this with
	{
		Page = Page < 1 ? 1 : Page,
		PageSize = PageSize switch
		{
			< 1 => 20,
			_ when PageSize > maxPageSize => maxPageSize,
			_ => PageSize
		}
	};
}
