using System;
using CulinaCloud.BuildingBlocks.Common;

namespace Culina.CookBook.Domain.Entities
{
    public class RecipeMetadata : AuditableEntity
    {
        public Guid RecipeId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public Recipe Recipe { get; set; }
    }
}
