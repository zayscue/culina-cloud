using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeTags.Commands.DeleteRecipeTag
{
    public class DeleteRecipeTagCommandValidator : AbstractValidator<DeleteRecipeTagCommand>
    {
        public DeleteRecipeTagCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();
            RuleFor(c => c.TagId)
                .NotEmpty();
        }
    }
}