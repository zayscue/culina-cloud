using System;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.CookBook.Domain.Entities;

namespace CulinaCloud.CookBook.Application.RecipeImages.Queries.GetRecipeImages
{
    public class GetRecipeImagesResponse : IMapFrom<RecipeImage>
    {
        public Guid RecipeId { get; set; }
        public Guid ImageId { get; set; }
        public string Url { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RecipeImage, GetRecipeImagesResponse>()
                .ForMember(d => d.Url,
                    opt =>
                        opt.MapFrom(src => src.Image.Url));
        }
    }
}