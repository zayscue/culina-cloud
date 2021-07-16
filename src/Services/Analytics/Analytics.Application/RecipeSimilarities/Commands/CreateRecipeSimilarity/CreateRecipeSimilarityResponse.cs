using System;
using CulinaCloud.Analytics.Domain.Entities;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;

namespace CulinaCloud.Analytics.Application.RecipeSimilarities.Commands.CreateRecipeSimilarity
{
    public class CreateRecipeSimilarityResponse : IMapFrom<RecipeSimilarity>
    {
        public Guid RecipeId { get; set; }
        public Guid SimilarRecipeId { get; set; }
        public string SimilarityType { get; set; }
        public decimal SimilarityScore { get; set; }
    }
}
