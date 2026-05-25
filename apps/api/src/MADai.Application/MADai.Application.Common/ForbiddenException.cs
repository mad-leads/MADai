namespace MADai.Application.Common;

public class ForbiddenException : AppException
{
	public ForbiddenException(string message = "Access denied.")
		: base(message)
	{
	}
}
