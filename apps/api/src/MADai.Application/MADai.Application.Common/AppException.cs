using System;

namespace MADai.Application.Common;

public class AppException : Exception
{
	public AppException(string message)
		: base(message)
	{
	}

	public AppException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
