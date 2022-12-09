import * as axios from 'axios';
import { BASEAPIURL, AUDIENCE } from '../constants';
import GetAccessTokenCallback from '../types/api/getAccessTokenCallback';
import PaginatedListAPIResponse from '../types/api/paginatedListAPIResponse';
import RecipeAPIResponse from '../types/api/recipeAPIResponse';
import PaginatedListViewModel from '../types/view/paginatedListViewModel';
import RecipeCardViewModel from '../types/view/recipeCardViewModel';
import RecipeViewModel from '../types/view/recipeViewModel';
import FavoriteViewModel from '../types/view/favoriteViewModel';
import RecommendedRecipeListItem from '../types/models/recommendedRecipeListItem';
import RecentRecipeListItem from '../types/models/recentRecipeListItem';
import MyRecipeListItem from '../types/models/myRecipeListItem';
import FavoriteRecipeListItem from '../types/models/favoriteRecipeListItem';
import SearchRecipeListItem from '../types/models/searchRecipeListItem';
import PopularRecipeListItem from '../types/models/popularRecipeListItem';
import SimilarRecipeListItem from '../types/models/similarRecipeListItem';
import SearchTagListItem from '../types/models/searchTagListItem';
import SearchIngredientListItem from '../types/models/searchIngredientListItem';
import CreateRecipeCommand from '../types/commands/createRecipeCommand';
import UpdateRecipeCommand from '../types/commands/updateRecipeCommand';
import CreateRecipeNutritionCommand from '../types/commands/createRecipeNutritionCommand';
import UpdateRecipeNutritionCommand from '../types/commands/updateRecipeNutritionCommand';
import UpdateRecipeStepsCommand from '../types/commands/updateRecipeStepsCommand';
import UpdateRecipeIngredientsCommand from '../types/commands/updateRecipeIngredientsCommand';
import CreateRecipeImageCommand from '../types/commands/createRecipeImageCommand';
import UpdateRecipeImagesCommand from '../types/commands/updateRecipeImagesCommand';
import CreateRecipeTagCommand from '../types/commands/createRecipeTagCommand';
import UpdateRecipeTagsCommand from '../types/commands/updateRecipeTagsCommand';
import CreateRecipeReviewCommand from '../types/commands/createRecipeReviewCommand';
import Recipe from '../types/models/recipe';
import RecipeNutrition from '../types/models/recipeNutrition';
import RecipeStep from '../types/models/recipeStep';
import RecipeIngredient from '../types/models/recipeIngredient';
import RecipeImage from '../types/models/recipeImage';
import RecipeTag from '../types/models/recipeTag';
import RecipeReview from '../types/models/recipeReview';
import ProfileInfo from '../types/models/profileInfo';
import ApplicationStatistics from '../types/models/applicationStatistics';

const APIClient = (
  getAccessTokenSilently: GetAccessTokenCallback
) => {
  const client = axios.default.create({
    baseURL: BASEAPIURL
  });

  client.interceptors.request.use(async (config: axios.AxiosRequestConfig) => {
    const requestConfig = { ...config };
    const accessToken = await getAccessTokenSilently({
      audience: AUDIENCE
    });
    requestConfig.headers = {
      ...config.headers?.common?.toJSON(),
      'Authorization': `Bearer ${accessToken}`
    };
    return requestConfig;
  });
  return client;
};


const useCulina = (
  callback: GetAccessTokenCallback
) => {
  const client = APIClient(callback);

  return {
    getRecommendedRecipes: async(page: number, limit: number) : Promise<PaginatedListAPIResponse<RecipeAPIResponse<RecommendedRecipeListItem>>> => {
      const params = {
        page,
        limit
      };
      const route = '/recipes/recommended';
      const { data } : { data: PaginatedListAPIResponse<RecipeAPIResponse<RecommendedRecipeListItem>> } = await client.get(route, {
        params
      });
      return data;
    },

    getRecentRecipes: async(page: number, limit: number) : Promise<PaginatedListAPIResponse<RecipeAPIResponse<RecentRecipeListItem>>> => {
      const params = {
        page,
        limit
      };
      const route = '/recipes/recent';
      const { data } : { data: PaginatedListAPIResponse<RecipeAPIResponse<RecentRecipeListItem>> } = await client.get(route, {
        params
      });
      return data;
    },

    getMyRecipes: async(page: number, limit: number) : Promise<PaginatedListAPIResponse<RecipeAPIResponse<MyRecipeListItem>>> => {
      const params = {
        page,
        limit
      };
      const route = '/recipes/mine';
      const { data } : { data: PaginatedListAPIResponse<RecipeAPIResponse<MyRecipeListItem>> } = await client.get(route, {
        params
      });
      return data;
    },

    getFavoriteRecipes: async(page: number, limit: number) : Promise<PaginatedListAPIResponse<RecipeAPIResponse<FavoriteRecipeListItem>>> => {
      const params = {
        page,
        limit
      };
      const route = '/recipes/favorites';
      const { data } : { data: PaginatedListAPIResponse<RecipeAPIResponse<FavoriteRecipeListItem>> } = await client.get(route, {
        params
      });
      console.log(data);
      return data;
    },

    getPopularRecipes: async(page: number, limit: number) : Promise<PaginatedListAPIResponse<RecipeAPIResponse<PopularRecipeListItem>>> => {
      const params = {
        page,
        limit
      };
      const route = '/recipes/popular';
      const { data } : { data: PaginatedListAPIResponse<RecipeAPIResponse<PopularRecipeListItem>> } = await client.get(route, {
        params
      });
      return data;
    },

    searchRecipes: async(name: string, page: number, limit: number) : Promise<PaginatedListAPIResponse<RecipeAPIResponse<SearchRecipeListItem>>> => {
      const params = {
        page,
        limit,
        name
      };
      const route = '/recipes';
      const { data } : { data: PaginatedListAPIResponse<RecipeAPIResponse<SearchRecipeListItem>> } = await client.get(route, {
        params
      });
      return data;
    },

    createRecipe: async(command: CreateRecipeCommand) : Promise<RecipeAPIResponse<Recipe>> => {
      const route = 'recipes';
      const { data } : { data: RecipeAPIResponse<Recipe> } = await client.post(route, command);
      return data;
    },

    getRecipe: async(recipeId: string): Promise<RecipeViewModel> => {
      const route = `/recipes/${recipeId}`;
      const { data } : { data: RecipeAPIResponse<Recipe> } = await client.get(route);
      const viewModel : RecipeViewModel = {
        isAFavorite: data.policy.isAFavorite,
        rating: data.popularity?.ratingWeightedAverage,
        recipe: data.data
      };
      return viewModel;
    },

    updateRecipe: async(recipeId: string, command: UpdateRecipeCommand) : Promise<{}> => {
      const route = `recipes/${recipeId}`;
      const { data = {} } = await client.put(route, command);
      return data;
    },

    createRecipeNutrition: async(recipeId: string, command: CreateRecipeNutritionCommand) : Promise<RecipeAPIResponse<RecipeNutrition>> => {
      const route = `recipes/${recipeId}/nutrition`;
      const { data } : { data : RecipeAPIResponse<RecipeNutrition> } = await client.post(route, command);
      return data;
    },

    getRecipeNutrition: async(recipeId: string) : Promise<RecipeAPIResponse<RecipeNutrition>> => {
      const route = `recipes/${recipeId}/nutrition`;
      const { data } : { data : RecipeAPIResponse<RecipeNutrition> } = await client.get(route);
      return data;
    },

    updateRecipeNutriton: async(recipeId: string, command: UpdateRecipeNutritionCommand) : Promise<{}> => {
      const route = `recipes/${recipeId}/nutrition`;
      const { data = {} } = await client.put(route, command);
      return data;
    },

    getRecipeSteps: async(recipeId: string) : Promise<Array<RecipeAPIResponse<RecipeStep>>> => {
      const route = `recipes/${recipeId}/steps`;
      const { data } : { data: Array<RecipeAPIResponse<RecipeStep>> } = await client.get(route);
      return data;
    },

    getRecipeStep: async(recipeId: string, order: number) : Promise<RecipeAPIResponse<RecipeStep>> => {
      const route = `recipes/${recipeId}/steps/${order}`;
      const { data } : { data: RecipeAPIResponse<RecipeStep> } = await client.get(route);
      return data;
    },

    updateRecipeSteps: async(recipeId: string, commands: Array<UpdateRecipeStepsCommand>) : Promise<{}> => {
      const route = `recipes/${recipeId}/steps`;
      const { data = {} } = await client.put(route, commands);
      return data;
    },

    getRecipeIngredients: async(recipeId: string) : Promise<Array<RecipeAPIResponse<RecipeIngredient>>> => {
      const route = `recipes/${recipeId}/ingredients`;
      const { data } : { data: Array<RecipeAPIResponse<RecipeIngredient>> } = await client.get(route);
      return data;
    },

    createRecipeIngredient: async(recipeId: string) : Promise<RecipeAPIResponse<RecipeIngredient>> => {
      const route = `recipes/${recipeId}/ingredients`;
      const { data } : { data: RecipeAPIResponse<RecipeIngredient> } = await client.post(route);
      return data;
    },

    updateRecipeIngredients: async(recipeId: string, commands: Array<UpdateRecipeIngredientsCommand>) : Promise<{}> => {
      const route = `recipes/${recipeId}/ingredients`;
      const { data = {} } = await client.put(route, commands);
      return data;
    },

    getRecipeIngredient: async(recipeId: string, recipeIngredientId: string) : Promise<RecipeAPIResponse<RecipeIngredient>> => {
      const route = `recipes/${recipeId}/ingredients/${recipeIngredientId}`;
      const { data } : { data: RecipeAPIResponse<RecipeIngredient> } = await client.get(route);
      return data;
    },

    getRecipeImages: async(recipeId: string) : Promise<Array<RecipeAPIResponse<RecipeImage>>> => {
      const route = `recipes/${recipeId}/images`;
      const { data } : { data: Array<RecipeAPIResponse<RecipeImage>> } = await client.get(route);
      return data;
    },

    createRecipeImage: async(recipeId: string, command: CreateRecipeImageCommand) : Promise<RecipeAPIResponse<RecipeImage>> => {
      const route = `recipes/${recipeId}/images`;
      const { data } : { data: RecipeAPIResponse<RecipeImage> } = await client.post(route, command);
      return data;
    },

    updateRecipeImages: async(recipeId: string, commands: Array<UpdateRecipeImagesCommand>) : Promise<{}> => {
      const route = `recipes/${recipeId}/images`;
      const { data = {} } = await client.put(route, commands);
      return data;
    },

    getRecipeImage: async(recipeId: string, imageId: string) : Promise<RecipeAPIResponse<RecipeImage>> => {
      const route = `recipes/${recipeId}/images/${imageId}`;
      const { data } : { data: RecipeAPIResponse<RecipeImage> } = await client.get(route);
      return data;
    },

    getRecipeTags: async(recipeId: string) : Promise<Array<RecipeAPIResponse<RecipeTag>>> => {
      const route = `recipes/${recipeId}/tags`;
      const { data } : { data: Array<RecipeAPIResponse<RecipeTag>> } = await client.get(route);
      return data;
    },

    createRecipeTag: async(recipeId: string, command: CreateRecipeTagCommand) : Promise<RecipeAPIResponse<RecipeTag>> => {
      const route = `recipes/${recipeId}/tags`;
      const { data } : { data: RecipeAPIResponse<RecipeTag> } = await client.post(route, command);
      return data;
    },

    updateRecipeTags: async(recipeId: string, commands: Array<UpdateRecipeTagsCommand>) : Promise<{}> => {
      const route = `recipes/${recipeId}/tags`;
      const { data = {} } = await client.put(route, commands);
      return data;
    },

    getRecipeTag: async(recipeId: string, tagId: string) : Promise<RecipeAPIResponse<RecipeTag>> => {
      const route = `recipes/${recipeId}/tags/${tagId}`;
      const { data } : { data: RecipeAPIResponse<RecipeTag> } = await client.get(route);
      return data;
    },

    getSimilarRecipes: async(recipeId: string, page: number, limit: number) : Promise<PaginatedListAPIResponse<RecipeAPIResponse<SimilarRecipeListItem>>> => {
      const params = {
        page,
        limit
      };
      const route = `/recipes/${recipeId}/similar`;
      const { data } : { data: PaginatedListAPIResponse<RecipeAPIResponse<SimilarRecipeListItem>> } = await client.get(route, {
        params
      });
      return data;
    },

    getRecipeReviews: async(recipeId: string, page: number, limit: number) : Promise<PaginatedListAPIResponse<RecipeReview>> => {
      const params = {
        page,
        limit
      };
      const route = `/recipes/${recipeId}/reviews`;
      const { data } : { data: PaginatedListAPIResponse<RecipeReview> } = await client.get(route, {
        params
      });
      return data;
    },

    createRecipeReview: async(recipeId: string, command: CreateRecipeReviewCommand) : Promise<RecipeReview> => {
      const route = `recipes/${recipeId}/reviews`;
      const { data } : { data: RecipeReview } = await client.post(route, command);
      return data;
    },

    favoriteRecipe: async(recipeId: string): Promise<FavoriteViewModel> => {
      const route = `/recipes/${recipeId}/favorite`;
      const { data } : { data: FavoriteViewModel } = await client.post(route, {});
      return data;
    },

    unFavoriteRecipe: async(recipeId: string): Promise<FavoriteViewModel> => {
      const route = `/recipes/${recipeId}/un-favorite`;
      const { data } : { data: FavoriteViewModel } = await client.post(route, {});
      return data;
    },

    searchTags: async(name: string, page: number, limit: number) : Promise<PaginatedListAPIResponse<SearchTagListItem>> => {
      const params = {
        page,
        limit,
        name
      };
      const route = '/tags';
      const { data } : { data: PaginatedListAPIResponse<SearchTagListItem> } = await client.get(route, {
        params
      });
      return data;
    },

    searchIngredients: async(name: string, page: number, limit: number) : Promise<PaginatedListAPIResponse<SearchIngredientListItem>> => {
      const params = {
        page,
        limit,
        name
      };
      const route = '/tags';
      const { data } : { data: PaginatedListAPIResponse<SearchIngredientListItem> } = await client.get(route, {
        params
      });
      return data;
    },

    getProfileInfo: async() : Promise<ProfileInfo> => {
      const route = '/users/me';
      const { data } : { data: ProfileInfo } = await client.get(route);
      return data;
    },

    getApplicationStatistics: async() : Promise<ApplicationStatistics> => {
      const route = '/admin/statistics';
      const { data } : { data: ApplicationStatistics } = await client.get(route);
      return data;
    },

    uploadImage: async(form: FormData) : Promise<any> => {
      const route = '/images/upload';
      const { data } = await client.post(route, form);
      return data;
    }
  }
}

export { useCulina, APIClient };
