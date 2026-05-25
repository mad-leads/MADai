namespace MADai.Application.Common;

public class NotFoundException : AppException
{
	public NotFoundException(string entity, object key)
		: base($"Entity '{entity}' with key '{key}' was not found.")
	{
	}
}
