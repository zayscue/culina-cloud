using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeIngredients.Queries.GetRecipeIngredient
{
    public class GetRecipeIngredientQueryValidator : AbstractValidator<GetRecipeIngredientQuery>
    {
        public GetRecipeIngredientQueryValidator()
        {
            RuleFor(q => q.RecipeId)
                .NotEmpty();
            RuleFor(q => q.RecipeIngredientId)
                .NotEmpty();
        }
    }
}