using System;
using CulinaCloud.BuildingBlocks.Common;

namespace CulinaCloud.Analytics.Domain.Entities
{
    public class RecipeSimilarity : AuditableEntity
    {
        public Guid RecipeId { get; set; }
        public Guid SimilarRecipeId { get; set; }
        public string SimilarityType { get; set; }
        public decimal SimilarityScore { get; set; }
    }
}
