using FluentValidation;

namespace Culina.CookBook.Application.Tags.Commands.CreateTag
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(c => c.TagName)
                .NotEmpty()
                .MaximumLength(64);
        }
    }
}