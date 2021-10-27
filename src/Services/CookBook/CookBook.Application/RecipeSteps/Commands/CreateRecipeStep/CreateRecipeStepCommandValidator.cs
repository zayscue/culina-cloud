using FluentValidation;

namespace CulinaCloud.CookBook.Application.RecipeSteps.Commands.CreateRecipeStep
{
    public class CreateRecipeStepCommandValidator : AbstractValidator<CreateRecipeStepCommand>
    {
        public CreateRecipeStepCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.Instruction)
                .NotEmpty()
                .MaximumLength(2048);

            RuleFor(c => c.CreatedBy)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}