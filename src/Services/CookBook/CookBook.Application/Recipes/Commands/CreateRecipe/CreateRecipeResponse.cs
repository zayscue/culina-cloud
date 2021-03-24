﻿using System;
using System.Collections.Generic;
using AutoMapper;
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
        
        public IList<CreateRecipeResponseRecipeImage> Images { get; set; }
        public IList<CreateRecipeResponseRecipeMetadata> Metadata { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Recipe, CreateRecipeResponse>()
                .ForMember(d => d.Metadata,
                    opt =>
                        opt.Condition(((src, dest,
                            srcMember) => srcMember != null && srcMember.Count > 0)))
                .ForMember(d => d.Images,
                    opt =>
                        opt.Condition(((src, dest,
                            srcMember) => srcMember != null && srcMember.Count > 0)));
        }
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
        public string IngredientName { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<RecipeIngredient, CreateRecipeResponseRecipeIngredient>()
                .ForMember(d => d.IngredientName,
                    opt =>
                        opt.MapFrom(src => src.Ingredient.IngredientName));
        }
    }

    public class CreateRecipeResponseRecipeMetadata : IMapFrom<RecipeMetadata>
    {
        public Guid RecipeId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class CreateRecipeResponseRecipeImage : IMapFrom<RecipeImage>
    {
        public Guid RecipeId { get; set; }
        public Guid ImageId { get; set; }
        public string Url { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<RecipeImage, CreateRecipeResponseRecipeImage>()
                .ForMember(d => d.Url,
                    opt =>
                        opt.MapFrom(src => src.Image.Url));
        }
    }
}