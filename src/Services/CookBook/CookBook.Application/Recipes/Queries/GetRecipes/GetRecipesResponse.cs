using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.CookBook.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CulinaCloud.CookBook.Application.Recipes.Queries.GetRecipes
{
    public class GetRecipesResponse : IMapFrom<Recipe>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EstimatedMinutes { get; set; }
        public string Serves { get; set; }
        public string Yield { get; set; }
        public int NumberOfSteps { get; set; }
        public int NumberOfIngredients { get; set; }


        public IList<GetRecipesResponseRecipeImage> Images { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Recipe, GetRecipesResponse>()
                .ForMember(d => d.Images,
                    opt =>
                        opt.Condition(((src, dest,
                            srcMember) => srcMember != null && srcMember.Count > 0)));
        }
    }

    public class GetRecipesResponseRecipeImage : IMapFrom<RecipeImage>
    {
        public Guid RecipeId { get; set; }
        public Guid ImageId { get; set; }
        public string Url { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RecipeImage, GetRecipesResponseRecipeImage>()
                .ForMember(d => d.Url,
                    opt =>
                        opt.MapFrom(src => src.Image.Url));
        }
    }
}
