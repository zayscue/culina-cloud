using FluentValidation;

namespace CulinaCloud.CookBook.Application.Recipes.Queries.GetRecipes
{
    public class GetRecipeQueryValidator : AbstractValidator<GetRecipesQuery>
    {
        public GetRecipeQueryValidator()
        {
            RuleFor(q => q.Limit)
                .LessThanOrEqualTo(1000);

            RuleFor(q => q.Name)
                .NotEmpty()
                .When(q => q.RecipeIds.Count == 0);

            RuleFor(q => q.RecipeIds)
                .NotEmpty()
                .When(r => string.IsNullOrWhiteSpace(r.Name));
        }
    }
}
