using FluentValidation;

namespace Culina.CookBook.Application.Recipes.Queries.GetRecipe
{
    public class GetRecipeQueryValidator : AbstractValidator<GetRecipeQuery>
    {
        public GetRecipeQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}