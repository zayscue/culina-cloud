using System;
using CulinaCloud.BuildingBlocks.Common;

namespace Culina.CookBook.Domain.Entities
{
    public class RecipeTag : AuditableEntity
    {
        public Guid RecipeId { get; set; }
        public Guid TagId { get; set; }

        public Recipe Recipe { get; set; }
        public Tag Tag { get; set; }
    }
}
