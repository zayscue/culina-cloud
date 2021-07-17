using FluentValidation;

namespace CulinaCloud.Interactions.Application.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.UserId)
                .MaximumLength(128);

            RuleFor(c => c.Comments)
                .MaximumLength(1024);

            RuleFor(c => c.Rating)
                .LessThanOrEqualTo(5)
                .GreaterThanOrEqualTo(0);
        }
    }
}
