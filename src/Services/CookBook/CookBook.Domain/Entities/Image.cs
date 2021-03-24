using System;
using System.Collections.Generic;
using CulinaCloud.BuildingBlocks.Common;

namespace Culina.CookBook.Domain.Entities
{
    public class Image  : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Url { get; set; }

        public IList<RecipeImage> RecipeImages { get; private set; } = new List<RecipeImage>();
    }
}