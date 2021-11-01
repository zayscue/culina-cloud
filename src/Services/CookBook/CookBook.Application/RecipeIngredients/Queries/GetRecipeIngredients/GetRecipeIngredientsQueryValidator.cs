using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeIngredients.Queries.GetRecipeIngredients
{
    public class GetRecipeIngredientsQueryValidator : AbstractValidator<GetRecipeIngredientsQuery>
    {
        public GetRecipeIngredientsQueryValidator()
        {
            RuleFor(q => q.RecipeId)
                .NotEmpty();
        }
    }
}