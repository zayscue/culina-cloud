using Culina.CookBook.Application.Common.Mapping;
using Culina.CookBook.Domain.Entities;
using System;

namespace Culina.CookBook.Application.Ingredients.Queries.GetIngredients
{
    public class GetIngredientsResponse : IMapFrom<Ingredient>
    {
        public Guid Id { get; set; }
        public string IngredientName { get; set; }
    }
}
