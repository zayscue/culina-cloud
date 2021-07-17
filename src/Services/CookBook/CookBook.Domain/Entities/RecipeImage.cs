using System;
using CulinaCloud.BuildingBlocks.Common;

namespace CulinaCloud.CookBook.Domain.Entities
{
    public class RecipeImage : AuditableEntity
    {
        public Guid RecipeId { get; set; }
        public Guid ImageId { get; set; }

        public Recipe Recipe { get; set; }
        public Image Image { get; set; }
    }
}
