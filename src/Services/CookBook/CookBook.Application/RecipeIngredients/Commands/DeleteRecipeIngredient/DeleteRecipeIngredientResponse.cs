using System;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.CookBook.Domain.Entities;

namespace CulinaCloud.CookBook.Application.RecipeIngredients.Commands.DeleteRecipeIngredient
{
    public class DeleteRecipeIngredientResponse : IMapFrom<RecipeIngredient>
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public Guid? IngredientId { get; set; }
        public string Quantity { get; set; }
        public string Part { get; set; }
        public string IngredientName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RecipeIngredient, DeleteRecipeIngredientResponse>()
                .ForMember(d => d.IngredientName,
                    opt =>
                        opt.MapFrom(src => src.Ingredient.IngredientName));
        }
    }
}