namespace CulinaCloud.Analytics.Application.Interfaces
{
    public interface ICollaborativeFilteringModel
    {
        float? PredictRecipeRecommendationScore(string userId, string recipeId);
    }
}
