import Recipe from "../models/recipe";

type RecipeViewModel = {
  isAFavorite: boolean,
  rating?: number,
  recipe: Recipe
};

export default RecipeViewModel;