using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeSteps.Commands.DeleteRecipeStep
{
    public class DeleteRecipeStepCommandValidator : AbstractValidator<DeleteRecipeStepCommand>
    {
        public DeleteRecipeStepCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.Order)
                .NotEmpty();
        }
    }
}