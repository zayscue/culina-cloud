type RecentRecipeListItem = {
  id: string,
  name: string,
  estimatedMinutes: number,
  serves: string,
  images: Array<{imageId: string, url: string}>,
  popularityScore: number,
  submitted: Date
}

export default RecentRecipeListItem;