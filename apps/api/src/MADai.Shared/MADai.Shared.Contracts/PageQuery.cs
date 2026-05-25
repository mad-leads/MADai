namespace MADai.Shared.Contracts;

public record PageQuery
{
	public int Page { get; init; } = 1;


	public int PageSize { get; init; } = 20;


	public string? Search { get; init; }

	public string? SortBy { get; init; }

	public bool SortDescending { get; init; }

	public PageQuery Normalize(int maxPageSize = 200) => this with
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
