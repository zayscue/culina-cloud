using FluentValidation;

namespace CulinaCloud.Analytics.Application.RecipePopularities.Commands.UpdateRecipePopularity
{
    public class UpdateRecipePopularityCommandValidator : AbstractValidator<UpdateRecipePopularityCommand>
    {
        public UpdateRecipePopularityCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.Rating)
                .GreaterThanOrEqualTo(0);

            RuleFor(c => c.LastModifiedBy)
                .NotEmpty();
        }
    }
}
