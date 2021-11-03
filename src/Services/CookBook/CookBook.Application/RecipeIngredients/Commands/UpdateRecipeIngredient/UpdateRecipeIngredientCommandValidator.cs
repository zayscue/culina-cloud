using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeIngredients.Commands.UpdateRecipeIngredient
{
    public class UpdateRecipeIngredientCommandValidator : AbstractValidator<UpdateRecipeIngredientCommand>
    {
        public UpdateRecipeIngredientCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();
            RuleFor(c => c.RecipeIngredientId)
                .NotEmpty();
            RuleFor(c => c.Quantity)
                .MaximumLength(32);
            RuleFor(c => c.Part)
                .NotEmpty()
                .MaximumLength(512);
            RuleFor(c => c.IngredientName)
                .MaximumLength(128);
            RuleFor(c => c.LastModifiedBy)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}