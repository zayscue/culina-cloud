using System;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.CookBook.Domain.Entities;

namespace CulinaCloud.CookBook.Application.RecipeTags.Commands.CreateRecipeTag
{
    public class CreateRecipeTagResponse : IMapFrom<RecipeTag>
    {
        public Guid RecipeId { get; set; }
        public Guid TagId { get; set; }
        public string TagName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RecipeTag, CreateRecipeTagResponse>()
                .ForMember(d => d.TagName,
                    opt =>
                        opt.MapFrom(src => src.Tag.TagName));
        }
    }
}