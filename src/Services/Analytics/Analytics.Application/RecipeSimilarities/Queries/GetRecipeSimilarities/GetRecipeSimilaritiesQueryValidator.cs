using FluentValidation;

namespace CulinaCloud.Analytics.Application.RecipeSimilarities.Queries.GetRecipeSimilarities
{
    public class GetRecipeSimilaritiesQueryValidator : AbstractValidator<GetRecipeSimilaritiesQuery>
    {
        public GetRecipeSimilaritiesQueryValidator()
        {
            RuleFor(q => q.RecipeId)
                .NotEmpty();

            RuleFor(q => q.Limit)
                .LessThanOrEqualTo(1000);
        }
    }
}
