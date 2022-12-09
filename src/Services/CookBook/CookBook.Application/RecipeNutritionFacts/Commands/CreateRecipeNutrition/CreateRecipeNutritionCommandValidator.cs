using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeNutritionFacts.Commands.CreateRecipeNutrition
{
    public class CreateRecipeNutritionCommandValidator : AbstractValidator<CreateRecipeNutritionCommand>
    {
        public CreateRecipeNutritionCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.ServingSize)
                .MaximumLength(32)
                .NotEmpty();

            RuleFor(c => c.ServingsPerRecipe)
                .NotEmpty();

            RuleFor(c => c.CreatedBy)
                .MaximumLength(128)
                .NotEmpty();
        }
    }
}