using System;
using System.Collections.Generic;
using CulinaCloud.BuildingBlocks.Common;

namespace CulinaCloud.CookBook.Domain.Entities
{
    public class Tag : AuditableEntity
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }

        public IList<RecipeTag> RecipeTags { get; private set; } = new List<RecipeTag>();
    }
}
