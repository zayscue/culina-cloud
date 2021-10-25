using FluentValidation;

namespace CulinaCloud.CookBook.Application.Tags.Commands.CreateTag
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(c => c.TagName)
                .NotEmpty()
                .MaximumLength(64);

            RuleFor(c => c.CreatedBy)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}