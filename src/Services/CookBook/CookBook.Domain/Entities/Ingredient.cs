using System;
using System.Collections.Generic;
using CulinaCloud.BuildingBlocks.Common;

namespace Culina.CookBook.Domain.Entities
{
    public class Ingredient : AuditableEntity
    {
        public Guid Id { get; set; }
        public string IngredientName { get; set; }

        public IList<RecipeIngredient> RecipeIngredients { get; private set; } = new List<RecipeIngredient>();
    }
}
