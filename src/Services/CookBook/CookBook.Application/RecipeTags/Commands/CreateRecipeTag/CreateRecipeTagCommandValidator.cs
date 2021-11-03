using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeTags.Commands.CreateRecipeTag
{
    public class CreateRecipeTagCommandValidator : AbstractValidator<CreateRecipeTagCommand>
    {
        public CreateRecipeTagCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();
            RuleFor(c => c.TagName)
                .NotEmpty()
                .MaximumLength(128);
            RuleFor(c => c.CreatedBy)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}