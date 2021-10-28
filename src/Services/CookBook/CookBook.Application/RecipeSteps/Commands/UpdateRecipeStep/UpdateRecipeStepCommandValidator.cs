using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeSteps.Commands.UpdateRecipeStep
{
    public class UpdateRecipeStepCommandValidator : AbstractValidator<UpdateRecipeStepCommand>
    {
        public UpdateRecipeStepCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.Order)
                .NotEmpty()
                .NotEqual(0);

            RuleFor(c => c.Instruction)
                .NotEmpty()
                .MaximumLength(2048);

            RuleFor(c => c.LastModifiedBy)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}