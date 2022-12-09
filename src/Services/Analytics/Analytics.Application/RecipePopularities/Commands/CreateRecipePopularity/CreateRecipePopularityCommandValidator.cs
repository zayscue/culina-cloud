using FluentValidation;

namespace CulinaCloud.Analytics.Application.RecipePopularities.Commands.CreateRecipePopularity
{
    public class CreateRecipePopularityCommandValidator : AbstractValidator<CreateRecipePopularityCommand>
    {
        public CreateRecipePopularityCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.RatingCount)
                .GreaterThanOrEqualTo(0);

            RuleFor(c => c.RatingSum)
                .GreaterThanOrEqualTo(0);

            RuleFor(c => c.CreatedBy)
                .NotEmpty();

            When(c => c.RatingAverage.HasValue, () =>
            {
                RuleFor(c => c.RatingAverage.Value)
                    .GreaterThanOrEqualTo(0);
            });

            When(c => c.RatingAverage.HasValue, () =>
            {
                RuleFor(c => c.RatingWeightedAverage.Value)
                    .GreaterThanOrEqualTo(0);
            });
        }
    }
}
