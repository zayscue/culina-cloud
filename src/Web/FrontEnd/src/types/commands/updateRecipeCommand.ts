import UpdateRecipeNutritionCommand from './updateRecipeNutritionCommand';
import UpdateRecipeImagesCommand from './updateRecipeImagesCommand';
import UpdateRecipeIngredientsCommand from './updateRecipeIngredientsCommand';
import UpdateRecipeTagsCommand from './updateRecipeTagsCommand';
import UpdateRecipeStepsCommand from './updateRecipeStepsCommand';

type UpdateRecipeCommand = {
  name: string,
  description: string,
  estimatedMinutes: number,
  serves: string,
  yield?: string,
  nutrition? : UpdateRecipeNutritionCommand,
  images? : Array<UpdateRecipeImagesCommand>,
  ingredients? : Array<UpdateRecipeIngredientsCommand>,
  tags?: Array<UpdateRecipeTagsCommand>
  steps?: Array<UpdateRecipeStepsCommand>
};

export default UpdateRecipeCommand;
