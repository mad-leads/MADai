using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace MADai.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (!_validators.Any())
		{
			return await next();
		}
		ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);
		List<ValidationFailure> failures = (from f in (await Task.WhenAll(_validators.Select((IValidator<TRequest> v) => v.ValidateAsync(context, cancellationToken)))).SelectMany((ValidationResult r) => r.Errors)
			where f != null
			select f).ToList();
		if (failures.Count != 0)
		{
			throw new ValidationFailedException(failures.Select((ValidationFailure f) => f.PropertyName + ": " + f.ErrorMessage));
		}
		return await next();
	}
}
