type FavoriteRecipeListItem = {
  id: string,
  name: string,
  estimatedMinutes: number,
  serves: string,
  images: Array<{imageId: string, url: string}>
}

export default FavoriteRecipeListItem;