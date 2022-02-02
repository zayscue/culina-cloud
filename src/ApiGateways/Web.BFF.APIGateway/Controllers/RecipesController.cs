using System.Collections;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CulinaCloud.Web.BFF.APIGateway.Controllers;

[Route("recipes")]
public class RecipesController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ICookBookService _cookBookService;
    private readonly IUsersService _usersService;
    private readonly IAnalyticsService _analyticsService;

    public RecipesController(
        ICurrentUserService currentUserService,
        ICookBookService cookBookService,
        IUsersService usersService,
        IAnalyticsService analyticsService)
    {
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _cookBookService = cookBookService ?? throw new ArgumentNullException(nameof(cookBookService));
        _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        _analyticsService = analyticsService ?? throw new ArgumentNullException(nameof(analyticsService));
    }

    [HttpGet("personal-feed")]
    public async Task<ActionResult> GetRecipeFeed([FromQuery] int page = 1, [FromQuery] int limit = 24)
    {
        var userId = _currentUserService.UserId;
        var recipeRecommendations = await _analyticsService.GetPersonalizedRecipeRecommendationsAsync(
            userId, page, limit);
        recipeRecommendations.Items ??= new List<RecipeRecommendationDto>();
        var recipeIds = recipeRecommendations.Items.Select(x => x.RecipeId).ToList();
        
        var recipeFavorites = await _usersService.GetUsersFavoritesAsync(
            userId, recipeIds, 1, limit);
        var favoritesDict = recipeRecommendations.Items
            .ToDictionary(x => x.RecipeId, x => false);
        if (recipeFavorites.Items is {Count: > 0})
        {
            foreach (var favoriteRecipeId in recipeFavorites.Items)
            {
                favoritesDict[favoriteRecipeId] = true;
            }   
        }

        var recipes = await _cookBookService.GetRecipesAsync(
            recipeIds, 1, limit);
        var recipesDict = recipes.Items?
            .ToDictionary(x => x.Id, x => x) ?? new Dictionary<Guid, RecipesDto>();
        
        var feedItems = recipeRecommendations.Items.Select(x => 
            new
            {
                Id = x.RecipeId,
                recipesDict[x.RecipeId].Name ,
                recipesDict[x.RecipeId].EstimatedMinutes,
                IsAFavorite = favoritesDict[x.RecipeId],
                recipesDict[x.RecipeId].Serves,
                recipesDict[x.RecipeId].Yield,
                x.PopularityScore,
                x.PredictedScore,
                UserId = userId
            }
        ).ToList();
        var feed = new
        {
            Items = feedItems,
            recipeRecommendations.Page,
            recipeRecommendations.TotalCount,
            recipeRecommendations.TotalPages
        };

        return Ok(feed);
    }

    [HttpGet("{recipeId:guid}")]
    public async Task<ActionResult> GetRecipe([FromRoute] Guid recipeId)
    {
        var user = _currentUserService.UserId;
        var recipe = await _cookBookService.GetRecipeAsync(recipeId);
        
        var recipeIds = new List<Guid>(new [] { recipeId } );
        var favorite = await _usersService.GetUsersFavoritesAsync(user, recipeIds, 1, 1);
        var isAFavorite = favorite.Items?.Count > 0;

        return Ok(new { recipe, isAFavorite });
    }

    [HttpGet("popular")]
    public async Task<ActionResult> GetPopularRecipes([FromQuery] string orderBy = "",
        [FromQuery] int page = 1, [FromQuery] int limit = 100)
    {
        var popularRecipes = await _analyticsService.GetPopularRecipesAsync(
            orderBy, page, limit);
        popularRecipes.Items ??= new List<RecipePopularityDto>();
        var popularRecipeIds = popularRecipes.Items.Select(x => x.RecipeId).ToList();
        
        var recipes = await _cookBookService.GetRecipesAsync(popularRecipeIds, 1, limit);
        recipes.Items ??= new List<RecipesDto>();
        var recipesDict = recipes.Items.ToDictionary(x => x.Id, x => x);

        var items = popularRecipes.Items.Select(x => new
        {
            Id = x.RecipeId,
            recipesDict[x.RecipeId].Name,
            recipesDict[x.RecipeId].EstimatedMinutes,
            recipesDict[x.RecipeId].Serves,
            recipesDict[x.RecipeId].Yield,
            x.RatingAverage,
            x.RatingCount,
            x.RatingSum,
            x.RatingWeightedAverage
        });
        return Ok(new
        {
            Items = items,
            popularRecipes.Page,
            popularRecipes.TotalCount,
            popularRecipes.TotalPages
        });
    }

    [HttpGet("{recipeId:guid}/similar")]
    public async Task<ActionResult> GetSimilarRecipes([FromRoute] Guid recipeId,
        [FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        var similarRecipes = await _analyticsService.GetSimilarRecipesAsync(recipeId, page, limit);
        similarRecipes.Items ??= new List<RecipeSimilarityDto>();
        var similarRecipeIds = similarRecipes.Items.Select(x => x.SimilarRecipeId).ToList();

        var recipes = await _cookBookService.GetRecipesAsync(similarRecipeIds, 1, limit);
        recipes.Items ??= new List<RecipesDto>();
        var recipesDict = recipes.Items.ToDictionary(x => x.Id, x => x);

        var items = similarRecipes.Items.Select(x => new
        {
            Id = x.SimilarRecipeId,
            recipesDict[x.SimilarRecipeId].Name,
            recipesDict[x.SimilarRecipeId].EstimatedMinutes,
            recipesDict[x.SimilarRecipeId].Serves,
            recipesDict[x.SimilarRecipeId].Yield,
            SimilarTo = x.RecipeId,
            x.SimilarityScore,
            x.PopularityScore
        });
        return Ok(new
        {
            Items = items,
            similarRecipes.Page,
            similarRecipes.TotalCount,
            similarRecipes.TotalPages
        });
    }
    
}