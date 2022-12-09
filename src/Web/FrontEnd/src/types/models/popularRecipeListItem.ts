type PopularRecipeListItem = {
  id: string,
  name: string,
  estimatedMinutes: number,
  serves: string,
  images: Array<{imageId: string, url: string}>,
  ratingAverage: number,
  ratingCount: number,
  ratingSum: number,
  ratingWeightedAverage: number
};

export default PopularRecipeListItem;
