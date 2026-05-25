using System.Collections.Generic;
using System.Linq;

namespace MADai.Application.Common;

public class ValidationFailedException : AppException
{
	public IReadOnlyList<string> Errors { get; }

	public ValidationFailedException(IEnumerable<string> errors)
		: base("Validation failed.")
	{
		Errors = errors.ToList();
	}
}
