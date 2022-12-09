import RecipeImage from '../models/recipeImage';

type RecipeSearchResult = {
  id: string;
  name: string;
  description: string;
  serves: string;
  estimatedMinutes: number;
  numberOfIngredients: number;
  numberOfSteps: number;
  yield: string;
  images: Array<RecipeImage>;
};

export default RecipeSearchResult;