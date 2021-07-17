using FluentValidation;

namespace CulinaCloud.Interactions.Application.Reviews.Queries.GetReviews
{
    public class GetReviewsQueryValidator : AbstractValidator<GetReviewsQuery>
    {
        public GetReviewsQueryValidator()
        {
            RuleFor(q => q.Limit)
                .LessThanOrEqualTo(1000);

            RuleFor(q => q.RecipeId)
                .NotEmpty();
        }
    }
}
