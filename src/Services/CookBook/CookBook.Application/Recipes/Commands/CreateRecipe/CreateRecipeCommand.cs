using System;
using System.Collections.Generic;
using MediatR;

namespace Culina.CookBook.Application.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommand : IRequest<CreateRecipeResponse>
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EstimatedMinutes { get; set; }
        public string Serves { get; set; }
        public string Yield { get; set; }

        public IList<string> Steps { get; set; }
        public IList<CreateRecipeCommandRecipeIngredient> Ingredients { get; set; }
        
        public IList<string> ImageUrls { get; set; }
        public IList<CreateRecipeCommandRecipeMetadata> Metadata { get; set; }
    }

    public class CreateRecipeCommandRecipeMetadata
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class CreateRecipeCommandRecipeIngredient
    {
        public string Quantity { get; set; }
        public string Part { get; set; }
        public string Type { get; set; }
    }
}