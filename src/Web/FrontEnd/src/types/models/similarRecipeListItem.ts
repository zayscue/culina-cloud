type SimilarRecipeListItem = {
  id: string,
  name: string,
  estimatedMinutes: number,
  serves: string,
  images: Array<{imageId: string, url: string}>,
  similarTo: string,
  similarityScore: number,
  popularityScore: number
}

export default SimilarRecipeListItem;
