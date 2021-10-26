using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeImages.Commands.DeleteRecipeImage
{
    public class DeleteRecipeImageCommandValidator : AbstractValidator<DeleteRecipeImageCommand>
    {
        public DeleteRecipeImageCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.ImageId)
                .NotEmpty();
        }
    }
}