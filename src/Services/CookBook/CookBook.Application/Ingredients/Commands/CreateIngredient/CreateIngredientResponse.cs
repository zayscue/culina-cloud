using System;
using Culina.CookBook.Domain.Entities;
using Culina.CookBook.Application.Common.Mapping;

namespace Culina.CookBook.Application.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientResponse : IMapFrom<Ingredient>
    {
        public Guid Id { get; set; }
        public string IngredientName { get; set; }
    }
}
