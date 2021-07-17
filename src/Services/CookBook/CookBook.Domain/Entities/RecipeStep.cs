using System;
using CulinaCloud.BuildingBlocks.Common;

namespace CulinaCloud.CookBook.Domain.Entities
{
    public class RecipeStep : AuditableEntity
    {
        public Guid RecipeId { get; set; }
        public int Order { get; set; }
        public string Instruction { get; set; }

        public Recipe Recipe { get; set; }
    }
}
