namespace CulinaCloud.Web.BFF.APIGateway.Controllers;

[Route("recipes")]
public class RecipesController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ICookBookService _cookBookService;
    private readonly IUsersService _usersService;
    private readonly IAnalyticsService _analyticsService;
    private readonly IInteractionsService _interactionsService;

    public RecipesController(
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

    [HttpGet("personal-feed")]
    public async Task<ActionResult> GetPersonalRecipeFeed([FromQuery] string? user = null,
        [FromQuery] int page = 1, [FromQuery] int limit = 24)
    {
        var userId = user ?? _currentUserService.UserId;
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
                Images = recipesDict[x.RecipeId].Images?.Select(i => new { i.ImageId, i.Url }).ToList() ?? null,
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

    [HttpGet("favorite-recipes")]
    public async Task<ActionResult> GetFavoriteRecipes([FromQuery] string? user = null,
        [FromQuery] int page = 1, [FromQuery] int limit = 24)
    {
        var userId = user ?? _currentUserService.UserId;
        var favoriteRecipes = await _usersService.GetUsersFavoritesAsync(
            userId, 1, limit);
        favoriteRecipes.Items ??= new List<Guid>();

        var recipes = await _cookBookService.GetRecipesAsync(favoriteRecipes.Items,
            page, limit);
        recipes.Items ??= new List<RecipesDto>();

        var items = recipes.Items.Select(x => new
        {
            x.Id,
            x.Name,
            x.EstimatedMinutes,
            x.Serves,
            x.Yield,
            Images = x.Images?.Select(i => new { i.ImageId, i.Url }).ToList() ?? null,
            IsAFavorite = true
        }).ToList();

        return Ok(new
        {
            Items = items,
            favoriteRecipes.Page,
            favoriteRecipes.TotalCount,
            favoriteRecipes.TotalPages
        });
    }

    [HttpGet("popular-recipes")]
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
            Images = recipesDict[x.RecipeId].Images?.Select(i => new { i.ImageId, i.Url }).ToList() ?? null,
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

    [HttpPost]
    public async Task<ActionResult> CreateRecipe([FromBody] RecipeDto recipe)
    {
        var user = _currentUserService.UserId;
        var createdRecipe = await _cookBookService.CreateRecipeAsync(recipe);
        var favorite = await _usersService.CreateFavoriteAsync(new FavoriteDto
        {
            RecipeId = createdRecipe.Id,
            UserId = user
        });
        return CreatedAtAction(
            nameof(GetRecipe), 
            new
            {
                recipeId = createdRecipe.Id
            },
            new
            {
                IsAFavorite = favorite != null,
                Recipe = new
                {
                    createdRecipe.Id,
                    createdRecipe.Name,
                    createdRecipe.Description,
                    createdRecipe.EstimatedMinutes,
                    createdRecipe.Serves,
                    createdRecipe.Yield,
                    createdRecipe.Nutrition,
                    Images = createdRecipe.Images?.Select(x => new
                    {
                        x.ImageId,
                        x.Url
                    }) ?? null,
                    createdRecipe.NumberOfIngredients,
                    Ingredients = createdRecipe.Ingredients?.Select(x =>  new
                    {
                        x.Part,
                        x.Quantity,
                        x.IngredientName,
                        x.IngredientId
                    }) ?? null,
                    createdRecipe.NumberOfSteps,
                    Steps = createdRecipe.Steps?.Select(x => new
                    {
                        x.Order,
                        x.Instruction
                    }) ?? null,
                    Tags = createdRecipe.Tags?.Select(x => new
                    {
                        x.TagId,
                        x.TagName
                    }) ?? null   
                }
            }
        );
    }
    
    [HttpGet("{recipeId:guid}")]
    public async Task<ActionResult> GetRecipe([FromRoute] Guid recipeId)
    {
        var user = _currentUserService.UserId;
        var recipe = await _cookBookService.GetRecipeAsync(recipeId);

        var recipeIds = new List<Guid>(new [] { recipeId } );
        var favorite = await _usersService.GetUsersFavoritesAsync(user, recipeIds, 1, 1);
        var isAFavorite = favorite.Items?.Count > 0;

        var response = new
        {
            IsAFavorite = isAFavorite,
            Recipe = new
            {
                recipe.Id,
                recipe.Name,
                recipe.Description,
                recipe.EstimatedMinutes,
                recipe.Serves,
                recipe.Yield,
                recipe.Nutrition,
                Images = recipe.Images?.Select(x => new
                {
                    x.ImageId,
                    x.Url
                }) ?? null,
                recipe.NumberOfIngredients,
                Ingredients = recipe.Ingredients?.Select(x =>  new
                {
                    x.Part,
                    x.Quantity,
                    x.IngredientName,
                    x.IngredientId
                }) ?? null,
                recipe.NumberOfSteps,
                Steps = recipe.Steps?.Select(x => new
                {
                    x.Order,
                    x.Instruction
                }) ?? null,
                Tags = recipe.Tags?.Select(x => new
                {
                    x.TagId,
                    x.TagName
                }) ?? null   
            }
        };
        return Ok(response);
    }

    [HttpPut("{recipeId:guid}")]
    public async Task<ActionResult> UpdateRecipe([FromRoute] Guid recipeId, [FromBody] RecipeDto recipe)
    {
        var user = _currentUserService.UserId;
        recipe.LastModifiedBy = user;

        await _cookBookService.UpdateRecipeAsync(recipeId, recipe);
        
        if (recipe.Nutrition != null)
        {
            recipe.Nutrition.LastModifiedBy = user;
            await _cookBookService.UpdateRecipeNutritionAsync(recipeId, recipe.Nutrition);
        }
        
        if (recipe.Images is {Count: > 0})
        {
            foreach (var image in recipe.Images)
            {
                image.CreatedBy = user;
            }

            await _cookBookService.BatchUpdateRecipeImagesAsync(recipeId, recipe.Images.ToList());
        }
        
        if (recipe.Ingredients is {Count: > 0})
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                ingredient.CreatedBy = user;
            }

            await _cookBookService.BatchUpdateRecipeIngredientsAsync(recipeId, recipe.Ingredients.ToList());
        }
        
        if (recipe.Tags is {Count: > 0})
        {
            foreach (var tag in recipe.Tags)
            {
                tag.CreatedBy = user;
            }

            await _cookBookService.BatchUpdateRecipeTagsAsync(recipeId, recipe.Tags.ToList());
        }
        
        if (recipe.Steps is {Count: > 0})
        {
            foreach (var step in recipe.Steps)
            {
                step.CreatedBy = user;
            }
            await _cookBookService.BatchUpdateRecipeStepsAsync(recipeId, recipe.Steps.ToList());
        }

        return Ok();
    }

    [HttpGet("{recipeId:guid}/nutrition")]
    public async Task<ActionResult> GetRecipeNutrition([FromRoute] Guid recipeId)
    {
        var nutrition = await _cookBookService.GetRecipeNutritionAsync(recipeId);
        return Ok(nutrition);
    }

    [HttpPut("{recipeId:guid}/nutrition")]
    public async Task<ActionResult> UpdateRecipeNutrition([FromRoute] Guid recipeId,
        [FromBody] RecipeNutritionDto nutrition)
    {
        await _cookBookService.UpdateRecipeNutritionAsync(recipeId, nutrition);
        return Ok();
    }

    [HttpGet("{recipeId:guid}/steps")]
    public async Task<ActionResult> GetRecipeSteps([FromRoute] Guid recipeId)
    {
        var steps = await _cookBookService.GetRecipeStepsAsync(recipeId);
        return Ok(steps);
    }

    [HttpPut("{recipeId:guid}/steps")]
    public async Task<ActionResult> BatchUpdateRecipeSteps([FromRoute] Guid recipeId,
        [FromBody] List<RecipeStepDto> steps)
    {
        var user = _currentUserService.UserId;
        foreach (var step in steps)
        {
            step.CreatedBy = user;
        }
        await _cookBookService.BatchUpdateRecipeStepsAsync(recipeId, steps);
        return Ok();
    }

    [HttpGet("{recipeId:guid}/ingredients")]
    public async Task<ActionResult> GetRecipeIngredients([FromRoute] Guid recipeId)
    {
        var ingredients = await _cookBookService.GetRecipeIngredientsAsync(recipeId);
        return Ok(ingredients);
    }

    [HttpPut("{recipeId:guid}/ingredients")]
    public async Task<ActionResult> BatchUpdateRecipeIngredients([FromRoute] Guid recipeId,
        [FromBody] List<RecipeIngredientDto> ingredients)
    {
        var user = _currentUserService.UserId;
        foreach (var ingredient in ingredients)
        {
            ingredient.CreatedBy = user;
        }
        await _cookBookService.BatchUpdateRecipeIngredientsAsync(recipeId, ingredients);
        return Ok();
    }

    [HttpPut("{recipeId:guid}/images")]
    public async Task<ActionResult> BatchUpdateRecipeImages([FromRoute] Guid recipeId,
        [FromBody] List<RecipeImageDto> images)
    {
        var user = _currentUserService.UserId;
        foreach (var image in images)
        {
            image.CreatedBy = user;
        }

        await _cookBookService.BatchUpdateRecipeImagesAsync(recipeId, images);
        return Ok();
    }

    [HttpPut("{recipeId:guid}/tags")]
    public async Task<ActionResult> BatchUpdateRecipeTags([FromRoute] Guid recipeId, [FromBody] List<RecipeTagDto> tags)
    {
        var user = _currentUserService.UserId;
        foreach (var tag in tags)
        {
            tag.CreatedBy = user;
        }

        await _cookBookService.BatchUpdateRecipeTagsAsync(recipeId, tags);
        return Ok();
    }


    [HttpGet("{recipeId:guid}/similar-recipes")]
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
            Images = recipesDict[x.SimilarRecipeId].Images?.Select(i => new { i.ImageId, i.Url }).ToList() ?? null,
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

    [HttpGet("{recipeId:guid}/reviews")]
    public async Task<ActionResult> GetRecipeReviews([FromRoute] Guid recipeId, [FromQuery] int page = 1, 
        [FromQuery] int limit = 10)
    {
        var recipeReviews = await _interactionsService.GetRecipeReviews(recipeId, page, limit);
        return Ok(recipeReviews);
    }

    [HttpPost("{recipeId:guid}/reviews")]
    public async Task<ActionResult> CreateRecipeReview([FromRoute] Guid recipeId, [FromBody] ReviewDto review)
    {
        var user = _currentUserService.UserId;
        review.RecipeId = recipeId;
        review.CreatedBy = user;
        review.UserId = user;
        var createdReview = await _interactionsService.CreateRecipeReviewAsync(review);
        return Ok(createdReview);
    }

    [HttpPost("{recipeId:guid}/favorite")]
    public async Task<ActionResult> FavoriteRecipe([FromRoute] Guid recipeId)
    {
        var userId = _currentUserService.UserId;
        var favorite = new FavoriteDto
        {
            RecipeId = recipeId,
            UserId = userId
        };
        await _usersService.CreateFavoriteAsync(favorite);
        return Ok();
    }

    [HttpPost("{recipeId:guid}/un-favorite")]
    public async Task<ActionResult> UnFavoriteRecipe([FromRoute] Guid recipeId)
    {
        var userId = _currentUserService.UserId;
        var favorite = new FavoriteDto
        {
            RecipeId = recipeId,
            UserId = userId
        };
        await _usersService.DeleteFavoriteAsync(favorite);
        return Ok();
    }
}