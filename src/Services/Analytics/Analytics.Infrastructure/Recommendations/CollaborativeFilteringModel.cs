using System;
using CulinaCloud.Analytics.Application.Interfaces;
using Microsoft.Extensions.ML;

namespace CulinaCloud.Analytics.Infrastructure.Recommendations
{
    public class CollaborativeFilteringModel : ICollaborativeFilteringModel
    {
        private readonly PredictionEnginePool<
            CollaborativeFilteringRecipeRecommendations.ModelInput,
            CollaborativeFilteringRecipeRecommendations.ModelOutput> _predictionEnginePool;

        public CollaborativeFilteringModel(
            PredictionEnginePool<CollaborativeFilteringRecipeRecommendations.ModelInput,
                CollaborativeFilteringRecipeRecommendations.ModelOutput> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }

        public float? PredictRecipeRecommendationScore(string userId, string recipeId)
        {
            var user_id = userId.Replace("auth0|", "");
            try
            {
                var output = _predictionEnginePool.Predict(new CollaborativeFilteringRecipeRecommendations.ModelInput
                {
                    Recipe_id = recipeId,
                    User_id = user_id
                });
                return output.Score;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}