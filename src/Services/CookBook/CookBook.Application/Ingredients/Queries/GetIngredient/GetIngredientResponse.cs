using System;
using Culina.CookBook.Application.Common.Mapping;
using Culina.CookBook.Domain.Entities;

namespace Culina.CookBook.Application.Ingredients.Queries.GetIngredient
{
    public class GetIngredientResponse : IMapFrom<Ingredient>
    {
        public Guid Id { get; set; }
        public string IngredientName { get; set; }
    }
}