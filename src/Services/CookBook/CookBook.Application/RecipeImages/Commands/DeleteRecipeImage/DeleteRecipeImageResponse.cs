using System;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.CookBook.Domain.Entities;

namespace CulinaCloud.CookBook.Application.RecipeImages.Commands.DeleteRecipeImage
{
    public class DeleteRecipeImageResponse : IMapFrom<RecipeImage>
    {
        public Guid RecipeId { get; set; }
        public Guid ImageId { get; set; }
        public string Url { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RecipeImage, DeleteRecipeImageResponse>()
                .ForMember(d => d.Url,
                    opt =>
                        opt.MapFrom(src => src.Image.Url));
        }
    }
}