using System.Collections.Generic;

namespace MADai.Shared.Contracts;

public sealed record ApiResult<T>(bool Success, T? Data, string? Error, IReadOnlyList<string>? Issues = null)
{
	public static ApiResult<T> Ok(T data)
	{
		return new ApiResult<T>(Success: true, data, null);
	}

	public static ApiResult<T> Fail(string error, IReadOnlyList<string>? issues = null)
	{
		return new ApiResult<T>(Success: false, default(T), error, issues);
	}
}
public sealed record ApiResult(bool Success, string? Error, IReadOnlyList<string>? Issues = null)
{
	public static ApiResult Ok()
	{
		return new ApiResult(Success: true, null);
	}

	public static ApiResult Fail(string error, IReadOnlyList<string>? issues = null)
	{
		return new ApiResult(Success: false, error, issues);
	}
}
