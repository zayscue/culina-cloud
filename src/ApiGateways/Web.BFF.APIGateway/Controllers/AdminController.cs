namespace CulinaCloud.Web.BFF.APIGateway.Controllers
{
    [Route("admin")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ICookBookService _cookBookService;
        private readonly IUsersService _usersService;
        private readonly IAnalyticsService _analyticsService;
        private readonly IInteractionsService _interactionsService;

        public AdminController(
            ICurrentUserService currentUserService,
            ICookBookService cookBookService,
            IUsersService usersService,
            IAnalyticsService analyticsService,
            IInteractionsService interactionsService)
        {
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _cookBookService = cookBookService ?? throw new ArgumentNullException(nameof(cookBookService));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _analyticsService = analyticsService ?? throw new ArgumentNullException(nameof(analyticsService));
            _interactionsService = interactionsService ?? throw new ArgumentNullException(nameof(interactionsService));
        }

        [HttpGet("statistics")]
        [Authorize("read:statistics")]
        public async Task<ActionResult> GetApplicationStatistics()
        {
            try
            {
                var cancellationToken = new CancellationToken();
                var recipePopularityStatisticsTask = _analyticsService.GetRecipePopularityStatisticsAsync(cancellationToken);
                var recipeStatisticsTask = _cookBookService.GetRecipeStatisticsAsync(cancellationToken);
                var userStatisticsTask = _usersService.GetUserStatisticsAsync(cancellationToken);
                var recipePopularityStatistics = await recipePopularityStatisticsTask;
                var recipeIds = recipePopularityStatistics.MostPopularRecipes.Select(x => x.RecipeId).ToList();
                var recipesTask = _cookBookService.GetRecipesAsync(recipeIds, 1, recipeIds.Count(), cancellationToken);
                var recipeStatistics = await recipeStatisticsTask;
                var userStatistics = await userStatisticsTask;
                var recipes = await recipesTask;
                if (recipes.Items == null)
                {
                    return StatusCode(500);
                }
                var recipeDictionary = recipes.Items.ToDictionary(x => x.Id, x => x.Name);
                return Ok(new
                {
                    RecipePopularityStatistics = new
                    {
                        recipePopularityStatistics.TotalHistoricalRecipes,
                        recipePopularityStatistics.TotalHistoricalReviews,
                        MostPopularRecipes = recipePopularityStatistics.MostPopularRecipes.Select(x => new
                        {
                            x.RecipeId,
                            RecipeName = recipeDictionary[x.RecipeId],
                            x.RatingAverage,
                            x.RatingCount,
                            x.RatingSum,
                            x.RatingWeightedAverage
                        })
                    },
                    RecipeStatistics = recipeStatistics,
                    UserStatistics = userStatistics
                });
            }
            catch(Exception)
            {
                return StatusCode(500); 
            }
        }
    }
}
