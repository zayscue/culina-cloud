using FluentValidation;

namespace CulinaCloud.CookBook.Application.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommandValidator : AbstractValidator<UpdateRecipeCommand>
    {
        public UpdateRecipeCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty();

            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(128);
            
            RuleFor(c => c.Description)
                .MaximumLength(8192);
            
            RuleFor(c => c.EstimatedMinutes)
                .NotEmpty();

            RuleFor(c => c.Serves)
                .MaximumLength(16);

            RuleFor(c => c.Yield)
                .MaximumLength(16);

            RuleFor(c => c.LastModifiedBy)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}