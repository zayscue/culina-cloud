using System.Collections;

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


    [HttpGet("get-feed")]
    public async Task<ActionResult<PaginatedListDto<RecipeFeedItemDto>>> GetRecipeFeed([FromQuery] string user,
        [FromQuery] int page = 1, [FromQuery] int limit = 24)
    {
        var userId = !string.IsNullOrWhiteSpace(user) ? user : _currentUserService.UserId;
        var recipeRecommendations = await _analyticsService.GetPersonalizedRecipeRecommendationsAsync(
            userId, page, limit);
        if (recipeRecommendations == null)
        {
            return BadRequest("An issue has occurred try to determine the user's personalized recipe recommendations");
        }
        var recipeIds = recipeRecommendations.Items.Select(x => x.RecipeId).ToList();


        var recipeFavorites = await _usersService.GetUsersFavoritesAsync(
            userId, recipeIds, 1, limit);
        if (recipeFavorites == null)
        {
            return BadRequest("An issue has occurred trying to determine the user's favorite recipes");
        }
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
        if (recipes == null)
        {
            return BadRequest("An issue has occurred trying to look up the recipe details");
        }
        var recipesDict = recipes.Items
            .ToDictionary(x => x.Id, x => x);
        
        var feedItems = recipeRecommendations.Items.Select(x => 
            new RecipeFeedItemDto
            {
                Id = x.RecipeId,
                Name = recipesDict[x.RecipeId].Name ,
                EstimatedMinutes = recipesDict[x.RecipeId].EstimatedMinutes,
                IsAFavorite = favoritesDict[x.RecipeId],
                Serves = recipesDict[x.RecipeId].Serves,
                Yield = recipesDict[x.RecipeId].Yield,
                PopularityScore = x.PopularityScore,
                PredictedScore = x.PredictedScore,
                UserId = userId
            }
        ).ToList();
        var result = new PaginatedListDto<RecipeFeedItemDto>
        {
            Items = feedItems,
            Page = recipeRecommendations.Page,
            TotalCount = recipeRecommendations.TotalCount,
            TotalPages = recipeRecommendations.TotalPages
        };

        return Ok(result);
    }
    
}