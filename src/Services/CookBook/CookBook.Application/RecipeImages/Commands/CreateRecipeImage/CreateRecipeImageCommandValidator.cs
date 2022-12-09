using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeImages.Commands.CreateRecipeImage
{
    public class CreateRecipeImageCommandValidator : AbstractValidator<CreateRecipeImageCommand>
    {
        public CreateRecipeImageCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.Url)
                .NotEmpty()
                .MaximumLength(1024);

            RuleFor(c => c.CreatedBy)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}