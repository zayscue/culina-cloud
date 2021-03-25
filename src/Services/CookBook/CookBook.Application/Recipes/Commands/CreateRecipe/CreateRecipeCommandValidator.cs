using System;
using System.Collections.Generic;
using System.Data;
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
                .SetValidator(new CreateRecipeCommandRecipeStepValidator());

            RuleForEach(c => c.Steps)
                .NotEmpty();

            RuleFor(c => c.Ingredients)
                .Must(x => x.Count > 0);
            
            RuleForEach(c => c.Ingredients)
                .SetValidator(new CreateRecipeCommandRecipeIngredientValidator());

            RuleForEach(c => c.Metadata)
                .SetValidator(new CreateRecipeCommandRecipeMetadataValidator());

            RuleFor(c => c.Metadata)
                .Must(c =>
                    c.Distinct(new CreateRecipeCommandRecipeMetadataComparer()).Count() == c.Count)
                .WithMessage("The metadata elements are not unique.");

            RuleFor(c => c.ImageUrls)
                .Must(c =>
                    c == null || c.Distinct().Count() == c.Count)
                .WithMessage("The image urls are not unique.");
            
            RuleForEach(c => c.ImageUrls)
                .SetValidator(new CreateRecipeCommandRecipeImageUrlValidator());
            
            RuleFor(c => c.Tags)
                .Must(c =>
                    c == null || c.Distinct().Count() == c.Count)
                .WithMessage("The image urls are not unique.");
            
            RuleForEach(c => c.Tags)
                .SetValidator(new CreateRecipeCommandRecipeTagValidator());
        }
        
        private class CreateRecipeCommandRecipeIngredientValidator : AbstractValidator<CreateRecipeCommandRecipeIngredient>
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

        private class CreateRecipeCommandRecipeMetadataValidator : AbstractValidator<CreateRecipeCommandRecipeMetadata>
        {
            public CreateRecipeCommandRecipeMetadataValidator()
            {
                RuleFor(c => c.Type)
                    .MaximumLength(64);

                RuleFor(c => c.Value)
                    .MaximumLength(128);
            }
        }
    
        private class CreateRecipeCommandRecipeMetadataComparer : IEqualityComparer<CreateRecipeCommandRecipeMetadata>
        {
            public bool Equals(CreateRecipeCommandRecipeMetadata x, CreateRecipeCommandRecipeMetadata y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Type == y.Type;
            }

            public int GetHashCode(CreateRecipeCommandRecipeMetadata obj)
            {
                return HashCode.Combine(obj.Type);
            }
        }
        
        private class CreateRecipeCommandRecipeStepValidator : AbstractValidator<string>
        {
            public CreateRecipeCommandRecipeStepValidator()
            {
                RuleFor(c => c)
                    .MaximumLength(512);
            }
        }
        
        private class CreateRecipeCommandRecipeImageUrlValidator : AbstractValidator<string>
        {
            public CreateRecipeCommandRecipeImageUrlValidator()
            {
                RuleFor(c => c)
                    .MaximumLength(1024);
            }
        }
        
        private class CreateRecipeCommandRecipeTagValidator : AbstractValidator<string>
        {
            public CreateRecipeCommandRecipeTagValidator()
            {
                RuleFor(c => c)
                    .MaximumLength(64);
            }
        }
    }
}