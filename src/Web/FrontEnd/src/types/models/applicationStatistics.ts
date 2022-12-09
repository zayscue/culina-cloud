type RecipePopularityStatistics = {
  totalHistoricalRecipes: number,
  totalHistoricalReviews: number,
  mostPopularRecipes: Array<{
    recipeId: string,
    recipeName: string,
    ratingAverage: number,
    ratingCount: number,
    ratingSum: number,
    ratingWeightedAverage: number
  }>
};

type RecipeStatistics = {
  totalRecipes: number,
  mostPopularTags: Array<{
    tagName: string,
    totalRecipeTags: number
  }>,
  mostPopularIngredients: Array<{
    ingredientName: string,
    totalIngredientReferences: number
  }>,
  dailyRecipeStatistics: Array<{
    newRecipes: number,
    date: Date
  }>
}

type UserStatistics = {
  totalActiveApplicationUsers: number,
  dailyApplicationUsersStatistics: Array<{
    logins: number,
    signUps: number,
    date: Date
  }>
}

type ApplicationStatistics = {
  recipePopularityStatistics: RecipePopularityStatistics,
  recipeStatistics: RecipeStatistics,
  userStatistics: UserStatistics
};

export default ApplicationStatistics;
