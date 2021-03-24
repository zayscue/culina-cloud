using System.Data;
using FluentValidation;

namespace Culina.CookBook.Application.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
    {
        public CreateRecipeCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(c => c.Description)
                .MaximumLength(512);

            RuleFor(c => c.EstimatedMinutes)
                .NotEmpty();

            RuleFor(c => c.Serves)
                .MaximumLength(16);

            RuleFor(c => c.Yield)
                .MaximumLength(16);

            RuleFor(c => c.Steps)
                .Must(x => x.Count > 0);

            RuleForEach(c => c.Steps)
                .NotEmpty();

            RuleFor(c => c.Ingredients)
                .Must(x => x.Count > 0);
            
            RuleForEach(c => c.Ingredients)
                .SetValidator(new CreateRecipeCommandRecipeIngredientValidator());

            RuleForEach(c => c.Metadata)
                .SetValidator(new CreateRecipeCommandRecipeMetadataValidator());
        }
    }

    public class CreateRecipeCommandRecipeIngredientValidator : AbstractValidator<CreateRecipeCommandRecipeIngredient>
    {
        public CreateRecipeCommandRecipeIngredientValidator()
        {
            RuleFor(c => c.Quantity)
                .MaximumLength(32);

            RuleFor(c => c.Part)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(c => c.Type)
                .MaximumLength(128);
        }
    }

    public class CreateRecipeCommandRecipeMetadataValidator : AbstractValidator<CreateRecipeCommandRecipeMetadata>
    {
        public CreateRecipeCommandRecipeMetadataValidator()
        {
            RuleFor(c => c.Type)
                .MaximumLength(64);

            RuleFor(c => c.Value)
                .MaximumLength(128);
        }
    }
}