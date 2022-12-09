using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeNutritionFacts.Commands.UpdateRecipeNutrition
{
    public class UpdateRecipeNutritionCommandValidator : AbstractValidator<UpdateRecipeNutritionCommand>
    {
        public UpdateRecipeNutritionCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.ServingSize)
                .MaximumLength(32)
                .NotEmpty();

            RuleFor(c => c.ServingsPerRecipe)
                .NotEmpty();

            RuleFor(c => c.LastModifiedBy)
                .MaximumLength(128)
                .NotEmpty();
        }
    }
}