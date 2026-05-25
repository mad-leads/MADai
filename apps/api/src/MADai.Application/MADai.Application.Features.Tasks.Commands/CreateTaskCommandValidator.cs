using FluentValidation;

namespace MADai.Application.Features.Tasks.Commands;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
	public CreateTaskCommandValidator()
	{
		RuleFor((CreateTaskCommand c) => c.Request.Title).NotEmpty().MaximumLength(200);
		RuleFor((CreateTaskCommand c) => c.Request.Description).MaximumLength(8000);
		RuleFor((CreateTaskCommand c) => c.Request.TimeoutSeconds).GreaterThan(0).When<CreateTaskCommand, int?>((CreateTaskCommand c) => c.Request.TimeoutSeconds.HasValue);
		RuleFor((CreateTaskCommand c) => c.Request.MaxRetries).GreaterThanOrEqualTo(0).When<CreateTaskCommand, int?>((CreateTaskCommand c) => c.Request.MaxRetries.HasValue);
		RuleFor((CreateTaskCommand c) => c.Request.CronExpression).NotEmpty().When<CreateTaskCommand, string>((CreateTaskCommand c) => c.Request.IsRecurring).WithMessage("Recurring tasks require a cron expression.");
	}
}
