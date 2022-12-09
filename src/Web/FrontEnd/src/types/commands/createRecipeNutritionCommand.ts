type CreateRecipeNutritionCommand = {
  calories: number;
  caloriesFromFat: number;
  caloriesFromFatPdv: number;
  cholesterol: number;
  cholesterolPdv: number;
  dietaryFiber: number;
  dietaryFiberPdv: number;
  protein: number;
  proteinPdv: number;
  recipeId: string;
  saturatedFat: number;
  saturatedFatPdv: number;
  servingSize: string;
  servingsPerRecipe: number;
  sodium: number;
  sodiumPdv: number;
  sugar: number;
  sugarPdv: number;
  totalCarbohydrates: number;
  totalCarbohydratesPdv: number;
  totalFat: number;
  totalFatPdv: number;
};

export default CreateRecipeNutritionCommand;
