using FluentValidation;

namespace MADai.Application.Features.ClaudeTasks.Commands;

public class CreateClaudeTaskCommandValidator : AbstractValidator<CreateClaudeTaskCommand>
{
	public CreateClaudeTaskCommandValidator()
	{
		RuleFor((CreateClaudeTaskCommand c) => c.Request.Title).NotEmpty().MaximumLength(300);
		RuleFor((CreateClaudeTaskCommand c) => c.Request.Description).MaximumLength(8000);
		RuleFor((CreateClaudeTaskCommand c) => c.Request.Notes).MaximumLength(8000);
	}
}
