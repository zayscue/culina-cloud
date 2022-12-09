import RecipeAPIPolicy from './recipeAPIPolicy';

type RecipePopularity = {
  recipeId: string,
  submitted: string,
  ratingCount: number,
  ratingSum: number,
  ratingAverage: number,
  ratingWeightedAverage: number
};

type RecipeAPIResponse<T> = {
  policy: RecipeAPIPolicy,
  data: T,
  popularity?: RecipePopularity
};

export default RecipeAPIResponse;