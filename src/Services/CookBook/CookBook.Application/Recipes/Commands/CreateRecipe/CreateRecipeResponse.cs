using System;
using System.Collections.Generic;
using Culina.CookBook.Application.Common.Mapping;
using Culina.CookBook.Domain.Entities;

namespace Culina.CookBook.Application.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeResponse : IMapFrom<Recipe>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EstimatedMinutes { get; set; }
        public string Serves { get; set; }
        public string Yield { get; set; }
        public int NumberOfSteps { get; set; }
        public int NumberOfIngredients { get; set; }

        public IList<CreateRecipeResponseRecipeStep> Steps { get; set; }
        public IList<CreateRecipeResponseRecipeIngredient> Ingredients { get; set; }
    }

    public class CreateRecipeResponseRecipeStep : IMapFrom<RecipeStep>
    {
        public Guid RecipeId { get; set; }
        public int Order { get; set; }
        public string Instruction { get; set; }
    }

    public class CreateRecipeResponseRecipeIngredient : IMapFrom<RecipeIngredient>
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public Guid? IngredientId { get; set; }
        public string Quantity { get; set; }
        public string Part { get; set; }
        
        public CreateRecipeResponseIngredient Ingredient { get; set; }
    }
    
    public class CreateRecipeResponseIngredient : IMapFrom<Ingredient>
    {
        public Guid Id { get; set; }
        public string IngredientName { get; set; }
    }
}