using System;
using System.Collections.Generic;
using System.Linq;
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
                .MaximumLength(8192);

            //RuleFor(c => c.EstimatedMinutes)
                //.NotEmpty();

            RuleFor(c => c.Serves)
                .MaximumLength(16);

            RuleFor(c => c.Yield)
                .MaximumLength(16);

            RuleFor(c => c.Steps)
                .Must(x => x.Count > 0);

            RuleForEach(c => c.Steps)
                .SetValidator(new CreateRecipeStepCommandValidator());

            RuleForEach(c => c.Steps)
                .NotEmpty();

            RuleFor(c => c.Ingredients)
                .Must(x => x.Count > 0);

            RuleForEach(c => c.Ingredients)
                .SetValidator(new CreateRecipeIngredientCommandValidator());

            RuleForEach(c => c.Metadata)
                .SetValidator(new CreateRecipeMetadataCommandValidator());

            RuleFor(c => c.Metadata)
                .Must(c =>
                    c == null || c.Distinct(new CreateRecipeMetadataCommandComparer()).Count() == c.Count)
                .WithMessage("The metadata elements are not unique.");

            RuleFor(c => c.ImageUrls)
                .Must(c =>
                    c == null || c.Distinct().Count() == c.Count)
                .WithMessage("The image urls are not unique.");

            RuleForEach(c => c.ImageUrls)
                .SetValidator(new CreateRecipeImageUrlCommandValidator());

            RuleFor(c => c.Tags)
                .Must(c =>
                    c == null || c.Distinct().Count() == c.Count)
                .WithMessage("The image urls are not unique.");

            RuleForEach(c => c.Tags)
                .SetValidator(new CreateRecipeTagCommandValidator());

            RuleFor(c => c.Nutrition)
                .SetValidator(new CreateRecipeNutritionCommandValidator());
        }

        private class CreateRecipeIngredientCommandValidator : AbstractValidator<CreateRecipeIngredientCommand>
        {
            public CreateRecipeIngredientCommandValidator()
            {
                RuleFor(c => c.Quantity)
                    .MaximumLength(32);

                RuleFor(c => c.Part)
                    .NotEmpty()
                    .MaximumLength(512);

                RuleFor(c => c.Type)
                    .MaximumLength(128);
            }
        }

        private class CreateRecipeMetadataCommandValidator : AbstractValidator<CreateRecipeMetadataCommand>
        {
            public CreateRecipeMetadataCommandValidator()
            {
                RuleFor(c => c.Type)
                    .MaximumLength(64);

                RuleFor(c => c.Value)
                    .MaximumLength(128);
            }
        }

        private class CreateRecipeMetadataCommandComparer : IEqualityComparer<CreateRecipeMetadataCommand>
        {
            public bool Equals(CreateRecipeMetadataCommand x, CreateRecipeMetadataCommand y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Type == y.Type;
            }

            public int GetHashCode(CreateRecipeMetadataCommand obj)
            {
                return HashCode.Combine(obj.Type);
            }
        }

        private class CreateRecipeStepCommandValidator : AbstractValidator<string>
        {
            public CreateRecipeStepCommandValidator()
            {
                RuleFor(c => c)
                    .MaximumLength(2048);
            }
        }

        private class CreateRecipeImageUrlCommandValidator : AbstractValidator<string>
        {
            public CreateRecipeImageUrlCommandValidator()
            {
                RuleFor(c => c)
                    .MaximumLength(1024);
            }
        }

        private class CreateRecipeTagCommandValidator : AbstractValidator<string>
        {
            public CreateRecipeTagCommandValidator()
            {
                RuleFor(c => c)
                    .MaximumLength(128);
            }
        }

        private class CreateRecipeNutritionCommandValidator : AbstractValidator<CreateRecipeNutritionCommand>
        {
            public CreateRecipeNutritionCommandValidator()
            {
                RuleFor(c => c.ServingSize)
                    .MaximumLength(32)
                    .NotEmpty();

                RuleFor(c => c.ServingsPerRecipe)
                    .NotEmpty();
            }
        }
    }
}