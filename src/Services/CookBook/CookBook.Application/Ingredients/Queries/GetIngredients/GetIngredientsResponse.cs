using System;
using CulinaCloud.CookBook.Application.Common.Mapping;
using CulinaCloud.CookBook.Domain.Entities;

namespace CulinaCloud.CookBook.Application.Ingredients.Queries.GetIngredients
{
    public class GetIngredientsResponse : IMapFrom<Ingredient>
    {
        public Guid Id { get; set; }
        public string IngredientName { get; set; }
    }
}
