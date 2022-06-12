using AutoMapper;
using CulinaCloud.Analytics.Domain.Entities;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using System;

namespace CulinaCloud.Analytics.Application.RecentRecipes.Queries.GetRecentRecipes
{
    public class RecentRecipeResponse : IMapFrom<RecipePopularity>
    {
        public Guid RecipeId { get; set; }
        public DateOnly Submitted { get; set; }
        public decimal PopularityScore { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RecipePopularity, RecentRecipeResponse>()
                .ForMember(d => d.RecipeId, opt => opt.MapFrom(src => src.RecipeId))
                .ForMember(d => d.Submitted, opt => opt.MapFrom(src => src.Submitted))
                .ForMember(d => d.PopularityScore, opt => opt.MapFrom(src => src.RatingWeightedAverage));
        }
    }
}
