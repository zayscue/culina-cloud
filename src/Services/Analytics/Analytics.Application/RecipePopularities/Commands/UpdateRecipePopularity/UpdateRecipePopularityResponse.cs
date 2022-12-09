using AutoMapper;
using CulinaCloud.Analytics.Domain.Entities;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using System;

namespace CulinaCloud.Analytics.Application.RecipePopularities.Commands.UpdateRecipePopularity
{
    public class UpdateRecipePopularityResponse : IMapFrom<RecipePopularity>
    {
        public Guid RecipeId { get; set; }
        public string Submitted { get; set; }
        public int RatingCount { get; set; }
        public int RatingSum { get; set; }
        public decimal RatingAverage { get; set; }
        public decimal RatingWeightedAverage { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RecipePopularity, UpdateRecipePopularityResponse>()
                .ForMember(d => d.Submitted,
                    opt =>
                        opt.MapFrom(src => src.Submitted.ToString("yyyy-MM-dd")));
        }
    }
}
