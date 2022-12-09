using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeSteps.Queries.GetRecipeStep
{
    public class GetRecipeStepQueryValidator : AbstractValidator<GetRecipeStepQuery>
    {
        public GetRecipeStepQueryValidator()
        {
            RuleFor(q => q.RecipeId)
                .NotEmpty();

            RuleFor(q => q.Order)
                .NotEmpty()
                .NotEqual(0);
        }
    }
}