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
        

        public IList<GetRecipeResponseRecipeImage> Images { get; set; }
        public GetRecipeResponseRecipeNutrition Nutrition { get; set; }
        public IList<GetRecipeResponseRecipeTag> Tags { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Recipe, GetRecipeResponse>()
                .ForMember(d => d.Images,
                    opt =>
                        opt.Condition(((src, dest,
                            srcMember) => srcMember != null && srcMember.Count > 0)))
                .ForMember(d => d.Tags,
                    opt =>
                        opt.Condition(((src, dest,
                            srcMember) => srcMember != null && srcMember.Count > 0)));
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
        public string IngredientName { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<RecipeIngredient, GetRecipeResponseRecipeIngredient>()
                .ForMember(d => d.IngredientName,
                    opt =>
                        opt.MapFrom(src => src.Ingredient.IngredientName));
        }
    }

    public class GetRecipeResponseRecipeImage : IMapFrom<RecipeImage>
    {
        public Guid RecipeId { get; set; }
        public Guid ImageId { get; set; }
        public string Url { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<RecipeImage, GetRecipeResponseRecipeImage>()
                .ForMember(d => d.Url,
                    opt =>
                        opt.MapFrom(src => src.Image.Url));
        }
    }
    
    public class GetRecipeResponseRecipeTag : IMapFrom<RecipeTag>
    {
        public Guid RecipeId { get; set; }
        public Guid TagId { get; set; }
        public string TagName { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<RecipeTag, GetRecipeResponseRecipeTag>()
                .ForMember(d => d.TagName,
                    opt =>
                        opt.MapFrom(src => src.Tag.TagName));
        }
    }
    
    public class GetRecipeResponseRecipeNutrition : IMapFrom<RecipeNutrition>
    {
        public Guid RecipeId { get; set; }
        public string ServingSize { get; set; }
        public int ServingsPerRecipe { get; set; }
        public decimal Calories { get; set; }
        public decimal CaloriesFromFat { get; set; }
        public decimal CaloriesFromFatPdv { get; set; }
        public decimal TotalFat { get; set; }
        public decimal TotalFatPdv { get; set; }
        public decimal SaturatedFat { get; set; }
        public decimal SaturatedFatPdv { get; set; }
        public decimal Cholesterol { get; set; }
        public decimal CholesterolPdv { get; set; }
        public decimal DietaryFiber { get; set; }
        public decimal DietaryFiberPdv { get; set; }
        public decimal Sugar { get; set; }
        public decimal SugarPdv { get; set; }
        public decimal Sodium { get; set; }
        public decimal SodiumPdv { get; set; }
        public decimal Protein { get; set; }
        public decimal ProteinPdv { get; set; }
        public decimal TotalCarbohydrates { get; set; }
        public decimal TotalCarbohydratesPdv { get; set; }
    }
}