using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeImages.Commands.UpdateRecipeImage
{
    public class UpdateRecipeImageCommandValidator : AbstractValidator<UpdateRecipeImageCommand>
    {
        public UpdateRecipeImageCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.ImageId)
                .NotEmpty();

            RuleFor(c => c.Url)
                .NotEmpty()
                .MaximumLength(1024);

            RuleFor(c => c.LastModifiedBy)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}