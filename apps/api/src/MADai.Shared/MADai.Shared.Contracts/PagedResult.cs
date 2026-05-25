using System;
using System.Collections.Generic;

namespace MADai.Shared.Contracts;

public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, long TotalCount)
{
	public int TotalPages
	{
		get
		{
			if (PageSize > 0)
			{
				return (int)Math.Ceiling((double)TotalCount / (double)PageSize);
			}
			return 0;
		}
	}

	public bool HasNext => Page < TotalPages;

	public bool HasPrevious => Page > 1;

	public static PagedResult<T> Empty(int page, int pageSize)
	{
		return new PagedResult<T>(Array.Empty<T>(), page, pageSize, 0L);
	}
}
