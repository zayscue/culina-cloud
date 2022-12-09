type CreateRecipeCommand = {
  name: string,
  description?: string,
  estimatedMinutes: number,
  serves?: string,
  yield?: string,
  nutrition?: {
    servingSize: string,
    servingsPerRecipe: number,
    calories?: number,
    caloriesFromFat?: number,
    caloriesFromFatPdv?: number,
    totalFat?: number,
    totalFatPdv?: number,
    saturatedFat?: number,
    saturatedFatPdv?: number,
    cholesterol?: number,
    cholesterolPdv?: number,
    dietaryFiber?: number,
    dietaryFiberPdv?: number,
    sugar?: number,
    sugarPdv?: number,
    sodium?: number,
    sodiumPdv?: number,
    protein?: number,
    proteinPdv?: number,
    totalCarbohydrates?: number,
    totalCarbohydratesPdv?: number
  },
  imageUrls?: Array<string>,
  ingredients: Array<{
    quantity? : string,
    part: string,
    type: string
  }>,
  steps: Array<string>,
  tags?: Array<string>
};

export default CreateRecipeCommand;