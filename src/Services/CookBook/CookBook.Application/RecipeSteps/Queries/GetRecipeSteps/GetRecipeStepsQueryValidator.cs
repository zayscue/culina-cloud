using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeSteps.Queries.GetRecipeSteps
{
    public class GetRecipeStepsQueryValidator : AbstractValidator<GetRecipeStepsQuery>
    {
        public GetRecipeStepsQueryValidator()
        {
            RuleFor(q => q.RecipeId)
                .NotEmpty();
        }
    }
}