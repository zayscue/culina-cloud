import RecipeNutrition from './recipeNutrition';
import RecipeStep from './recipeStep';
import RecipeIngredient from './recipeIngredient';
import RecipeImage from './recipeImage';
import RecipeTag from './recipeTag';

type Recipe = {
  id: string;
  name: string;
  description?: string;
  serves: string;
  estimatedMinutes: number;
  numberOfIngredients: number;
  numberOfSteps: number;
  yield?: string;
  images?: Array<RecipeImage>;
  ingredients: Array<RecipeIngredient>;
  nutrition?: RecipeNutrition;
  steps: Array<RecipeStep>;
  tags?: Array<RecipeTag>;
};

export default Recipe;