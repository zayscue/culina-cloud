namespace CulinaCloud.Web.BFF.APIGateway.Controllers;

[Route("favorites")]
public class FavoritesController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ICookBookService _cookBookService;
    private readonly IUsersService _usersService;

    public FavoritesController(ICurrentUserService currentUserService, ICookBookService cookBookService, IUsersService usersService)
    {
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _cookBookService = cookBookService ?? throw new ArgumentNullException(nameof(cookBookService));
        _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
    }

    public async Task<ActionResult<FavoriteDto>> Create([FromBody] FavoriteDto favorite)
    {
        var currentUserId = _currentUserService.UserId;
        favorite.UserId = currentUserId;
        var recipeExists = await RecipeExistsAsync(favorite.RecipeId);
        if (!recipeExists) return BadRequest($"{favorite.RecipeId} does not correspond to an existing recipe.");
        var createdFavorite = await _usersService.CreateFavoriteAsync(favorite);
        return Ok(createdFavorite);
    }

    private async Task<bool> RecipeExistsAsync(Guid recipeId)
    {
        try
        {
            var recipe = await _cookBookService.GetRecipeAsync(recipeId);
            return recipe != null;
        }
        catch (RecipeNotFoundException)
        {
            return false;
        }
    }
}