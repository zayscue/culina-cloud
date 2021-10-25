using System;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.CookBook.Domain.Entities;

namespace CulinaCloud.CookBook.Application.Ingredients.Queries.GetIngredient
{
    public class GetIngredientResponse : IMapFrom<Ingredient>
    {
        public Guid Id { get; set; }
        public string IngredientName { get; set; }
    }
}