using System;
using CulinaCloud.BuildingBlocks.Common;

namespace Culina.CookBook.Domain.Entities
{
    public class RecipeImage : AuditableEntity
    {
        public Guid RecipeId { get; set; }
        public string Url { get; set; }

        public Recipe Recipe { get; set; }
    }
}
