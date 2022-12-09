type RecipeCardViewModel = {
  id: string,
  name: string,
  estimatedMinutes: number,
  isAFavorite: boolean,
  serves: string,
  rating?: number,
  images: Array<{imageId: string, url: string}>
};

export default RecipeCardViewModel;
