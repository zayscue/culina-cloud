using FluentValidation;

namespace CulinaCloud.CookBook.Application.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientCommandValidator : AbstractValidator<CreateIngredientCommand>
    {
        public CreateIngredientCommandValidator()
        {
            RuleFor(c => c.IngredientName)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}
