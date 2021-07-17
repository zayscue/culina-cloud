using FluentValidation;

namespace CulinaCloud.Analytics.Application.RecipePopularities.Commands.CreateRecipePopularity
{
    public class CreateRecipePopularityCommandValidator : AbstractValidator<CreateRecipePopularityCommand>
    {
        public CreateRecipePopularityCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.Submitted)
                .NotEmpty();

            RuleFor(c => c.RatingCount)
                .GreaterThanOrEqualTo(0);

            RuleFor(c => c.RatingSum)
                .GreaterThanOrEqualTo(0);

            RuleFor(c => c.RatingAverage)
                .GreaterThanOrEqualTo(0);

            RuleFor(c => c.RatingWeightedAverage)
                .GreaterThanOrEqualTo(0);
        }
    }
}
