using FluentValidation;
using System.Collections.Generic;

namespace CulinaCloud.Analytics.Application.RecipeSimilarities.Commands.CreateRecipeSimilarity
{
    public class CreateRecipeSimilarityCommandValidator : AbstractValidator<CreateRecipeSimilarityCommand>
    {
        public CreateRecipeSimilarityCommandValidator()
        {
            RuleFor(c => c.RecipeId)
                .NotEmpty();

            RuleFor(c => c.SimilarRecipeId)
                .NotEmpty();

            var similarityTypes = new List<string>() { "tag", "ingredient" };
            RuleFor(c => c.SimilarityType)
                .Must(c => similarityTypes.Contains(c))
                .WithMessage($"The similarity type must be one of these values: {string.Join(",", similarityTypes)}");

            RuleFor(c => c.SimilarityScore)
                .GreaterThan(0);
        }
    }
}
