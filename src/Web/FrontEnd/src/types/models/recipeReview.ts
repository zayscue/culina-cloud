type RecipeReview = {
  id: string,
  recipeId: string,
  userId: string,
  rating: number,
  comments?: string
};

export default RecipeReview;
