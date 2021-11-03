using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeIngredients.Commands.DeleteRecipeIngredient
{
    public class DeleteRecipeIngredientCommandValidator : AbstractValidator<DeleteRecipeIngredientCommand>
    {
        public DeleteRecipeIngredientCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.RecipeIngredientId)
                .NotEmpty();
        }
    }
}