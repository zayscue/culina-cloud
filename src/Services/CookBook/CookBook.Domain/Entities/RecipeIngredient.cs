using System;
using CulinaCloud.BuildingBlocks.Common;

namespace Culina.CookBook.Domain.Entities
{
    public class RecipeIngredient : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public Guid? IngredientId { get; set; }
        public string Quantity { get; set; }
        public string Part { get; set; }

        public Recipe Recipe { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}
