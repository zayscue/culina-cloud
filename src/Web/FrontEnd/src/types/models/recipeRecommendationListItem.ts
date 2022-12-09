type RecipeRecommendationListItem = {
  policy: {
    isAFavorite: boolean,
    canEdit: boolean,
    isOwner: boolean
  }
  data: {
    id: string,
    name: string,
    estimatedMinutes: number,
    isAFavorite: boolean,
    serves: string,
    images: Array<{imageId: string, url: string}>,
    popularityScore: number,
    predictedScore: number,
    userId: string
  }
};

export default RecipeRecommendationListItem;