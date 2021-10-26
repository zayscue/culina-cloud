using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeNutritionFacts.Queries.GetRecipeNutrition
{
    public class GetRecipeNutritionQueryValidator : AbstractValidator<GetRecipeNutritionQuery>
    {
        public GetRecipeNutritionQueryValidator()
        {
            RuleFor(q => q.RecipeId)
                .NotEmpty();
        }
    }
}