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
                .GreaterThan(0);

            RuleFor(c => c.RatingSum)
                .GreaterThan(0);

            RuleFor(c => c.RatingAverage)
                .GreaterThan(0);

            RuleFor(c => c.RatingWeightedAverage)
                .GreaterThan(0);
        }
    }
}
