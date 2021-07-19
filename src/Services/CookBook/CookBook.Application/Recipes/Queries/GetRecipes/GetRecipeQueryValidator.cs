using FluentValidation;

namespace CulinaCloud.CookBook.Application.Recipes.Queries.GetRecipes
{
    public class GetRecipeQueryValidator : AbstractValidator<GetRecipesQuery>
    {
        public GetRecipeQueryValidator()
        {
            RuleFor(q => q.Limit)
                .LessThanOrEqualTo(100);

            RuleFor(q => q.Name)
                .NotEmpty();
        }
    }
}
