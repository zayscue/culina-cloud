using System;
using CulinaCloud.BuildingBlocks.Common;

namespace CulinaCloud.Analytics.Domain.Entities
{
    public class RecipePopularity : AuditableEntity
    {
        public Guid RecipeId { get; set; }
        public DateOnly Submitted { get; set; }
        public int RatingCount { get; set; }
        public int RatingSum { get; set; }
        public decimal RatingAverage { get; set; }
        public decimal RatingWeightedAverage { get; set; }
    }
}
