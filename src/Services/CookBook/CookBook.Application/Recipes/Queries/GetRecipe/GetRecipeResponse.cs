using System;
using System.Collections.Generic;
using AutoMapper;
using Culina.CookBook.Application.Common.Mapping;
using Culina.CookBook.Domain.Entities;

namespace Culina.CookBook.Application.Recipes.Queries.GetRecipe
{
    public class GetRecipeResponse : IMapFrom<Recipe>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EstimatedMinutes { get; set; }
        public string Serves { get; set; }
        public string Yield { get; set; }
        public int NumberOfSteps { get; set; }
        public int NumberOfIngredients { get; set; }

        public IList<GetRecipeResponseRecipeStep> Steps { get; set; }
        public IList<GetRecipeResponseRecipeIngredient> Ingredients { get; set; }
        
        public IList<GetRecipeResponseRecipeMetadata> Metadata { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Recipe, GetRecipeResponse>()
                .ForMember(d => d.Metadata, 
                    opt =>
                        opt.Condition(((src, dest, srcMember) => srcMember != null && srcMember.Count > 0)));
        }
    }

    public class GetRecipeResponseRecipeStep : IMapFrom<RecipeStep>
    {
        public Guid RecipeId { get; set; }
        public int Order { get; set; }
        public string Instruction { get; set; }
    }

    public class GetRecipeResponseRecipeIngredient : IMapFrom<RecipeIngredient>
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public Guid? IngredientId { get; set; }
        public string Quantity { get; set; }
        public string Part { get; set; }

        public GetRecipeResponseIngredient Ingredient { get; set; }
    }

    public class GetRecipeResponseIngredient : IMapFrom<Ingredient>
    {
        public Guid Id { get; set; }
        public string IngredientName { get; set; }
    }
    
    public class GetRecipeResponseRecipeMetadata : IMapFrom<RecipeMetadata>
    {
        public Guid RecipeId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}