import RecipePolicy  from '../api/recipeAPIPolicy';

type RecommendedRecipeListItem = {
  id: string,
  name: string,
  estimatedMinutes: number,
  serves: string,
  images: Array<{imageId: string, url: string}>,
  popularityScore: number,
  predictedScore?: number,
  userId: string
}

export default RecommendedRecipeListItem;