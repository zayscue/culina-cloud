using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeIngredients.Commands.CreateRecipeIngredient
{
    public class CreateRecipeIngredientCommandValidator : AbstractValidator<CreateRecipeIngredientCommand>
    {
        public CreateRecipeIngredientCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();
            RuleFor(c => c.Quantity)
                .MaximumLength(32);
            RuleFor(c => c.Part)
                .NotEmpty()
                .MaximumLength(512);
            RuleFor(c => c.IngredientName)
                .MaximumLength(128);
            RuleFor(c => c.CreatedBy)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}