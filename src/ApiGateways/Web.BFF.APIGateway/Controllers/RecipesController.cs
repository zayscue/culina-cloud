namespace CulinaCloud.Web.BFF.APIGateway.Controllers;

[Route("recipes")]
[Authorize]
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

    [HttpGet("recommended")]
    public async Task<ActionResult> GetPersonalRecipeFeed([FromQuery] int page = 1, [FromQuery] int limit = 24)
    {
        var userId = _currentUserService.UserId;
        var recipeRecommendations = await _analyticsService.GetPersonalizedRecipeRecommendationsAsync(
            userId, page, limit);
        recipeRecommendations.Items ??= new List<RecipeRecommendationDto>();
        var recipeIds = recipeRecommendations.Items.Select(x => x.RecipeId).ToList();

        var areRecipesFavoritesOfTheUserTask = AreRecipesFavoritesOfTheUser(recipeIds, userId);
        var IsUserOwnerOfTheRecipesTask = IsUserOwnerOfTheRecipes(recipeIds, userId);
        var canUserEditRecipesTask = CanUserEditRecipes(recipeIds, userId);
        var getRecipesTask = _cookBookService.GetRecipesAsync(recipeIds, 1, limit);

        var recipes = await getRecipesTask;
        var recipesDict = recipes.Items?
            .ToDictionary(x => x.Id, x => x) ?? new Dictionary<Guid, RecipesDto>();
        var favoritesDict = await areRecipesFavoritesOfTheUserTask;
        var recipeOwnersDict = await IsUserOwnerOfTheRecipesTask;
        var editableRecipesDict = await canUserEditRecipesTask;

        var feedItems = recipeRecommendations.Items.Select(x =>
            new RecipeAPIResponse
            {
                Policy = new RecipePolicy
                {
                    IsAFavorite = favoritesDict[x.RecipeId],
                    CanEdit = editableRecipesDict[x.RecipeId],
                    IsOwner = recipeOwnersDict[x.RecipeId]
                },
                Data = new
                {
                    Id = x.RecipeId,
                    recipesDict[x.RecipeId].Name,
                    recipesDict[x.RecipeId].EstimatedMinutes,
                    recipesDict[x.RecipeId].Serves,
                    recipesDict[x.RecipeId].Yield,
                    Images = recipesDict[x.RecipeId].Images?.Select(i => new { i.ImageId, i.Url }).ToList() ?? null,
                    x.PopularityScore,
                    x.PredictedScore,
                    UserId = userId
                }
            }
        ).ToList();

        return Ok(new PaginatedRecipeAPIResponse
        {
            Items = feedItems,
            Page = recipeRecommendations.Page,
            TotalCount = recipeRecommendations.TotalCount,
            TotalPages = recipeRecommendations.TotalPages
        });
    }

    [HttpGet("favorites")]
    public async Task<ActionResult> GetFavoriteRecipes([FromQuery] int page = 1, [FromQuery] int limit = 24)
    {
        var userId = _currentUserService.UserId;
        var favoriteRecipes = await _usersService.GetUsersFavoritesAsync(
            userId, page, limit);
        favoriteRecipes.Items ??= new List<FavoriteDto>();
        var recipeIds = favoriteRecipes.Items.Select(x => x.RecipeId).ToList();

        var areRecipesFavoritesOfTheUserTask = AreRecipesFavoritesOfTheUser(recipeIds, userId);
        var IsUserOwnerOfTheRecipesTask = IsUserOwnerOfTheRecipes(recipeIds, userId);
        var canUserEditRecipesTask = CanUserEditRecipes(recipeIds, userId);
        var getRecipesTask = _cookBookService.GetRecipesAsync(recipeIds, 1, limit);

        var recipes = await getRecipesTask;
        var favoritesDict = await areRecipesFavoritesOfTheUserTask;
        var recipeOwnersDict = await IsUserOwnerOfTheRecipesTask;
        var editableRecipesDict = await canUserEditRecipesTask;


        var items = recipes.Items?.Select(x =>
            new RecipeAPIResponse
            {
                Policy = new RecipePolicy
                {
                    IsAFavorite = favoritesDict[x.Id],
                    CanEdit = editableRecipesDict[x.Id],
                    IsOwner = recipeOwnersDict[x.Id]
                },
                Data = new
                {
                    x.Id,
                    x.Name,
                    x.EstimatedMinutes,
                    x.Serves,
                    x.Yield,
                    Images = x.Images?.Select(i => new { i.ImageId, i.Url }).ToList() ?? null
                }
            }
        ).ToList();

        return Ok(new PaginatedRecipeAPIResponse
        {
            Items = items,
            Page = favoriteRecipes.Page,
            TotalCount = favoriteRecipes.TotalCount,
            TotalPages = favoriteRecipes.TotalPages
        });
    }

    [HttpGet("popular")]
    public async Task<ActionResult> GetPopularRecipes([FromQuery] string orderBy = "",
        [FromQuery] int page = 1, [FromQuery] int limit = 100)
    {
        var userId = _currentUserService.UserId;
        var popularRecipes = await _analyticsService.GetPopularRecipesAsync(
            orderBy, page, limit);
        popularRecipes.Items ??= new List<RecipePopularityDto>();
        var recipeIds = popularRecipes.Items.Select(x => x.RecipeId).ToList();

        var areRecipesFavoritesOfTheUserTask = AreRecipesFavoritesOfTheUser(recipeIds, userId);
        var IsUserOwnerOfTheRecipesTask = IsUserOwnerOfTheRecipes(recipeIds, userId);
        var canUserEditRecipesTask = CanUserEditRecipes(recipeIds, userId);
        var getRecipesTask = _cookBookService.GetRecipesAsync(recipeIds, 1, limit);

        var recipes = await getRecipesTask;
        recipes.Items ??= new List<RecipesDto>();
        var recipesDict = recipes.Items.ToDictionary(x => x.Id, x => x);
        var favoritesDict = await areRecipesFavoritesOfTheUserTask;
        var recipeOwnersDict = await IsUserOwnerOfTheRecipesTask;
        var editableRecipesDict = await canUserEditRecipesTask;

        var items = popularRecipes.Items.Select(x =>
            new RecipeAPIResponse
            {
                Policy = new RecipePolicy
                {
                    IsAFavorite = favoritesDict[x.RecipeId],
                    CanEdit = editableRecipesDict[x.RecipeId],
                    IsOwner = recipeOwnersDict[x.RecipeId]
                },
                Data = new
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
                }
            }
        ).ToList();
        return Ok(new PaginatedRecipeAPIResponse
        {
            Items = items,
            Page = popularRecipes.Page,
            TotalCount = popularRecipes.TotalCount,
            TotalPages = popularRecipes.TotalPages
        });
    }

    [HttpPost]
    public async Task<ActionResult> CreateRecipe([FromBody] RecipeDto recipe)
    {
        var user = _currentUserService.UserId;
        var createdRecipe = await _cookBookService.CreateRecipeAsync(recipe);
        //var createFavoriteTask = _usersService.CreateFavoriteAsync(new FavoriteDto
        //{
        //    RecipeId = createdRecipe.Id,
        //    UserId = user
        //});
        var createRecipeEntitlementTask = _usersService.CreateRecipeEntitlementAsync(new RecipeEntitlementDto
        {
            RecipeId = createdRecipe.Id,
            UserId = user,
            GrantedBy = user,
            Type = "author"
        });

        //var createdFavorite = await createFavoriteTask;
        var createdRecipeEntitlement = await createRecipeEntitlementTask;

        return CreatedAtAction(
            nameof(GetRecipe),
            new
            {
                recipeId = createdRecipe.Id
            },
            new RecipeAPIResponse
            {
                Policy = new RecipePolicy
                {
                    IsAFavorite = false,
                    CanEdit = createdRecipeEntitlement != null,
                    IsOwner = createdRecipeEntitlement != null
                },
                Data = new
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
                        x.Id,
                        x.Quantity,
                        x.Part,
                        x.IngredientId,
                        x.IngredientName
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
        async Task<object?> GetData(Guid id)
        {
            var recipe = await _cookBookService.GetRecipeAsync(id);
            if (recipe == null)
            {
                return null;
            }
            return new
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
                Ingredients = recipe.Ingredients?.Select(x => new
                {
                    x.Id,
                    x.Quantity,
                    x.Part,
                    x.IngredientId,
                    x.IngredientName
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
            };
        };
        var userId = _currentUserService.UserId;
        var getDataTask = GetData(recipeId);
        var isFavoriteTask = IsRecipeAFavoriteOfTheUser(recipeId, userId);
        var canEditTask = CanUserEditRecipe(recipeId, userId);
        var isOwnerTask = IsUserOwnerOfTheRecipe(recipeId, userId);

        var getDataResult = await getDataTask;
        var isFavoriteResult = await isFavoriteTask;
        var canEditResult = await canEditTask;
        var isOwnerResult = await isOwnerTask;

        var response = new RecipeAPIResponse
        {
            Policy = new RecipePolicy
            {
                IsAFavorite = isFavoriteResult.Item2,
                CanEdit = canEditResult.Item2,
                IsOwner = isOwnerResult.Item2
            },
            Data = getDataResult
        };
        return Ok(response);
    }

    [HttpPut("{recipeId:guid}")]
    public async Task<ActionResult> UpdateRecipe([FromRoute] Guid recipeId, [FromBody] RecipeDto recipe)
    {
        var user = _currentUserService.UserId;
        var canEditResult = await CanUserEditRecipe(recipeId, user);
        if (!canEditResult.Item2) return Forbid();

        var updateTasks = new List<Task>();

        recipe.LastModifiedBy = user;
        var updateRecipeTask = _cookBookService.UpdateRecipeAsync(recipeId, recipe);
        updateTasks.Add(updateRecipeTask);


        if (recipe.Nutrition != null)
        {
            recipe.Nutrition.LastModifiedBy = user;
            var updateRecipeNutTask = _cookBookService.UpdateRecipeNutritionAsync(recipeId, recipe.Nutrition);
            updateTasks.Add(updateRecipeNutTask);
        }

        if (recipe.Images is {Count: > 0})
        {
            foreach (var image in recipe.Images)
            {
                image.CreatedBy = user;
            }
            var updateRecipeImagesTask = _cookBookService.BatchUpdateRecipeImagesAsync(recipeId, recipe.Images.ToList());
            updateTasks.Add(updateRecipeImagesTask);
        }

        if (recipe.Ingredients is {Count: > 0})
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                ingredient.RecipeId = recipeId;
                ingredient.CreatedBy = user;
            }
            var updateRecipeIngredientsTask = _cookBookService.BatchUpdateRecipeIngredientsAsync(recipeId, recipe.Ingredients.ToList());
            updateTasks.Add(updateRecipeIngredientsTask);
        }

        if (recipe.Tags is {Count: > 0})
        {
            foreach (var tag in recipe.Tags)
            {
                tag.CreatedBy = user;
            }
            var updateRecipeTagsTask = _cookBookService.BatchUpdateRecipeTagsAsync(recipeId, recipe.Tags.ToList());
            updateTasks.Add(updateRecipeTagsTask);
        }

        if (recipe.Steps is {Count: > 0})
        {
            foreach (var step in recipe.Steps)
            {
                step.CreatedBy = user;
            }
            var updateRecipeStepsTask = _cookBookService.BatchUpdateRecipeStepsAsync(recipeId, recipe.Steps.ToList());
            updateTasks.Add(updateRecipeStepsTask);
        }
        await Task.WhenAll(updateTasks);

        return Ok();
    }

    [HttpPost("{recipeId:guid}/nutrition")]
    public async Task<ActionResult> CreateRecipeNutrition([FromRoute] Guid recipeId,
        [FromBody] RecipeNutritionDto nutrition)
    {
        var user = _currentUserService.UserId;
        var canEditResult = await CanUserEditRecipe(recipeId, user);
        if (!canEditResult.Item2) return Forbid();
        var createdNutrition = await _cookBookService.CreateRecipeNutritionAsync(recipeId, nutrition);
        return CreatedAtAction(
            nameof(GetRecipeNutrition),
            new { },
            createdNutrition
        );
    }

    [HttpGet("{recipeId:guid}/nutrition")]
    public async Task<ActionResult> GetRecipeNutrition([FromRoute] Guid recipeId)
    {
        async Task<object?> GetData(Guid id)
        {
            var nutrition = await _cookBookService.GetRecipeNutritionAsync(id);
            return nutrition;
        };
        var userId = _currentUserService.UserId;
        var getDataTask = GetData(recipeId);
        var isFavoriteTask = IsRecipeAFavoriteOfTheUser(recipeId, userId);
        var canEditTask = CanUserEditRecipe(recipeId, userId);
        var isOwnerTask = IsUserOwnerOfTheRecipe(recipeId, userId);

        var getDataResult = await getDataTask;
        var isFavoriteResult = await isFavoriteTask;
        var canEditResult = await canEditTask;
        var isOwnerResult = await isOwnerTask;

        var response = new RecipeAPIResponse
        {
            Policy = new RecipePolicy
            {
                IsAFavorite = isFavoriteResult.Item2,
                CanEdit = canEditResult.Item2,
                IsOwner = isOwnerResult.Item2
            },
            Data = getDataResult
        };
        return Ok(response);
    }

    [HttpPut("{recipeId:guid}/nutrition")]
    public async Task<ActionResult> UpdateRecipeNutrition([FromRoute] Guid recipeId,
        [FromBody] RecipeNutritionDto nutrition)
    {
        var user = _currentUserService.UserId;
        var canEditResult = await CanUserEditRecipe(recipeId, user);
        if (!canEditResult.Item2) return Forbid();
        nutrition.LastModifiedBy = user;
        await _cookBookService.UpdateRecipeNutritionAsync(recipeId, nutrition);
        return Ok();
    }

    [HttpGet("{recipeId:guid}/steps")]
    public async Task<ActionResult> GetRecipeSteps([FromRoute] Guid recipeId)
    {
        var userId = _currentUserService.UserId;
        async Task<object?> GetData(Guid id)
        {
            var steps = await _cookBookService.GetRecipeStepsAsync(id);
            return steps;
        };
        var getDataTask = GetData(recipeId);
        var isFavoriteTask = IsRecipeAFavoriteOfTheUser(recipeId, userId);
        var canEditTask = CanUserEditRecipe(recipeId, userId);
        var isOwnerTask = IsUserOwnerOfTheRecipe(recipeId, userId);

        var getDataResult = await getDataTask;
        var isFavoriteResult = await isFavoriteTask;
        var canEditResult = await canEditTask;
        var isOwnerResult = await isOwnerTask;

        var response = new RecipeAPIResponse
        {
            Policy = new RecipePolicy
            {
                IsAFavorite = isFavoriteResult.Item2,
                CanEdit = canEditResult.Item2,
                IsOwner = isOwnerResult.Item2
            },
            Data = getDataResult
        };
        return Ok(response);
    }

    [HttpGet("{recipeId:guid}/steps/{order:int}")]
    public async Task<ActionResult> GetRecipeStep([FromRoute] Guid recipeId, [FromRoute] int order)
    {
        var userId = _currentUserService.UserId;
        async Task<object?> GetData(Guid id, int ix)
        {
            var recipeStep = await _cookBookService.GetRecipeStepAsync(id, ix);
            return recipeStep;
        };
        var getDataTask = GetData(recipeId, order);
        var isFavoriteTask = IsRecipeAFavoriteOfTheUser(recipeId, userId);
        var canEditTask = CanUserEditRecipe(recipeId, userId);
        var isOwnerTask = IsUserOwnerOfTheRecipe(recipeId, userId);

        var getDataResult = await getDataTask;
        var isFavoriteResult = await isFavoriteTask;
        var canEditResult = await canEditTask;
        var isOwnerResult = await isOwnerTask;

        var response = new RecipeAPIResponse
        {
            Policy = new RecipePolicy
            {
                IsAFavorite = isFavoriteResult.Item2,
                CanEdit = canEditResult.Item2,
                IsOwner = isOwnerResult.Item2
            },
            Data = getDataResult
        };
        return Ok(response);
    }

    [HttpPut("{recipeId:guid}/steps")]
    public async Task<ActionResult> BatchUpdateRecipeSteps([FromRoute] Guid recipeId,
        [FromBody] List<RecipeStepDto> steps)
    {
        var user = _currentUserService.UserId;
        var canEditResult = await CanUserEditRecipe(recipeId, user);
        if (!canEditResult.Item2) return Forbid();
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
        var userId = _currentUserService.UserId;
        async Task<object?> GetData(Guid id)
        {
            var ingredients = await _cookBookService.GetRecipeIngredientsAsync(id);
            return ingredients;
        };
        var getDataTask = GetData(recipeId);
        var isFavoriteTask = IsRecipeAFavoriteOfTheUser(recipeId, userId);
        var canEditTask = CanUserEditRecipe(recipeId, userId);
        var isOwnerTask = IsUserOwnerOfTheRecipe(recipeId, userId);

        var getDataResult = await getDataTask;
        var isFavoriteResult = await isFavoriteTask;
        var canEditResult = await canEditTask;
        var isOwnerResult = await isOwnerTask;

        var response = new RecipeAPIResponse
        {
            Policy = new RecipePolicy
            {
                IsAFavorite = isFavoriteResult.Item2,
                CanEdit = canEditResult.Item2,
                IsOwner = isOwnerResult.Item2
            },
            Data = getDataResult
        };
        return Ok(response);
    }

    [HttpPost("{recipeId:guid}/ingredients")]
    public async Task<ActionResult> CreateRecipeIngredient([FromRoute] Guid recipeId,
        [FromBody] RecipeIngredientDto recipeIngredient)
    {
        var user = _currentUserService.UserId;
        var canEditResult = await CanUserEditRecipe(recipeId, user);
        if (!canEditResult.Item2) return Forbid();
        recipeIngredient.CreatedBy = user;
        recipeIngredient.RecipeId = recipeId;
        var createdRecipeIngredient = await _cookBookService.CreateRecipeIngredientAsync(recipeId, recipeIngredient);
        return CreatedAtAction(
            nameof(GetRecipeIngredient),
            new {recipeId, recipeIngredientId = createdRecipeIngredient.Id},
            new
            {
                createdRecipeIngredient.Id,
                createdRecipeIngredient.Quantity,
                createdRecipeIngredient.Part,
                createdRecipeIngredient.IngredientId,
                createdRecipeIngredient.IngredientName
            });
    }

    [HttpPut("{recipeId:guid}/ingredients")]
    public async Task<ActionResult> BatchUpdateRecipeIngredients([FromRoute] Guid recipeId,
        [FromBody] List<RecipeIngredientDto> ingredients)
    {
        var user = _currentUserService.UserId;
        var canEditResult = await CanUserEditRecipe(recipeId, user);
        if (!canEditResult.Item2) return Forbid();
        foreach (var ingredient in ingredients)
        {
            ingredient.RecipeId = recipeId;
            ingredient.CreatedBy = user;
        }
        await _cookBookService.BatchUpdateRecipeIngredientsAsync(recipeId, ingredients);
        return Ok();
    }

    [HttpGet("{recipeId:guid}/ingredients/{recipeIngredientId:guid}")]
    public async Task<ActionResult> GetRecipeIngredient([FromRoute] Guid recipeId, [FromRoute] Guid recipeIngredientId)
    {
        var userId = _currentUserService.UserId;
        async Task<object?> GetData(Guid _recipeId, Guid _recipeIngredientId)
        {
            var recipeIngredient = await _cookBookService.GetRecipeIngredientAsync(_recipeId, _recipeIngredientId);
            return recipeIngredient;
        };
        var getDataTask = GetData(recipeId, recipeIngredientId);
        var isFavoriteTask = IsRecipeAFavoriteOfTheUser(recipeId, userId);
        var canEditTask = CanUserEditRecipe(recipeId, userId);
        var isOwnerTask = IsUserOwnerOfTheRecipe(recipeId, userId);

        var getDataResult = await getDataTask;
        var isFavoriteResult = await isFavoriteTask;
        var canEditResult = await canEditTask;
        var isOwnerResult = await isOwnerTask;

        var response = new RecipeAPIResponse
        {
            Policy = new RecipePolicy
            {
                IsAFavorite = isFavoriteResult.Item2,
                CanEdit = canEditResult.Item2,
                IsOwner = isOwnerResult.Item2
            },
            Data = getDataResult
        };
        return Ok(response);
    }

    [HttpGet("{recipeId:guid}/images")]
    public async Task<ActionResult> GetRecipeImages([FromRoute] Guid recipeId)
    {
        var userId = _currentUserService.UserId;
        async Task<object?> GetData(Guid _recipeId)
        {
            var images = await _cookBookService.GetRecipeImagesAsync(_recipeId);
            return images;
        };
        var getDataTask = GetData(recipeId);
        var isFavoriteTask = IsRecipeAFavoriteOfTheUser(recipeId, userId);
        var canEditTask = CanUserEditRecipe(recipeId, userId);
        var isOwnerTask = IsUserOwnerOfTheRecipe(recipeId, userId);

        var getDataResult = await getDataTask;
        var isFavoriteResult = await isFavoriteTask;
        var canEditResult = await canEditTask;
        var isOwnerResult = await isOwnerTask;

        var response = new RecipeAPIResponse
        {
            Policy = new RecipePolicy
            {
                IsAFavorite = isFavoriteResult.Item2,
                CanEdit = canEditResult.Item2,
                IsOwner = isOwnerResult.Item2
            },
            Data = getDataResult
        };
        return Ok(response);
    }

    [HttpPost("{recipeId:guid}/images")]
    public async Task<ActionResult> CreateRecipeImage([FromRoute] Guid recipeId, [FromBody] RecipeImageDto image)
    {
        var user = _currentUserService.UserId;
        var canEditResult = await CanUserEditRecipe(recipeId, user);
        if (!canEditResult.Item2) return Forbid();
        image.CreatedBy = user;
        var createdRecipeImage = await _cookBookService.CreateRecipeImageAsync(recipeId, image);
        return CreatedAtAction(
            nameof(GetRecipeImage),
            new {recipeId, imageId = image.ImageId},
            createdRecipeImage);
    }

    [HttpPut("{recipeId:guid}/images")]
    public async Task<ActionResult> BatchUpdateRecipeImages([FromRoute] Guid recipeId,
        [FromBody] List<RecipeImageDto> images)
    {
        var user = _currentUserService.UserId;
        var canEditResult = await CanUserEditRecipe(recipeId, user);
        if (!canEditResult.Item2) return Forbid();
        foreach (var image in images)
        {
            image.CreatedBy = user;
        }
        await _cookBookService.BatchUpdateRecipeImagesAsync(recipeId, images);
        return Ok();
    }

    [HttpGet("{recipeId:guid}/images/{imageId:guid}")]
    public async Task<ActionResult> GetRecipeImage([FromRoute] Guid recipeId, [FromRoute] Guid imageId)
    {
        var userId = _currentUserService.UserId;
        async Task<object?> GetData(Guid _recipeId, Guid _imageId)
        {
            var recipeImage = await _cookBookService.GetRecipeImageAsync(_recipeId, _imageId);
            return recipeImage;
        };
        var getDataTask = GetData(recipeId, imageId);
        var isFavoriteTask = IsRecipeAFavoriteOfTheUser(recipeId, userId);
        var canEditTask = CanUserEditRecipe(recipeId, userId);
        var isOwnerTask = IsUserOwnerOfTheRecipe(recipeId, userId);

        var getDataResult = await getDataTask;
        var isFavoriteResult = await isFavoriteTask;
        var canEditResult = await canEditTask;
        var isOwnerResult = await isOwnerTask;

        var response = new RecipeAPIResponse
        {
            Policy = new RecipePolicy
            {
                IsAFavorite = isFavoriteResult.Item2,
                CanEdit = canEditResult.Item2,
                IsOwner = isOwnerResult.Item2
            },
            Data = getDataResult
        };
        return Ok(response);
    }

    [HttpGet("{recipeId:guid}/tags")]
    public async Task<ActionResult> GetRecipeTags([FromRoute] Guid recipeId)
    {
        var userId = _currentUserService.UserId;
        async Task<object?> GetData(Guid _recipeId)
        {
            var tags = await _cookBookService.GetRecipeTagsAsync(_recipeId);
            return tags.Select(x => new
            {
                x.TagId,
                x.TagName
            });
        };
        var getDataTask = GetData(recipeId);
        var isFavoriteTask = IsRecipeAFavoriteOfTheUser(recipeId, userId);
        var canEditTask = CanUserEditRecipe(recipeId, userId);
        var isOwnerTask = IsUserOwnerOfTheRecipe(recipeId, userId);

        var getDataResult = await getDataTask;
        var isFavoriteResult = await isFavoriteTask;
        var canEditResult = await canEditTask;
        var isOwnerResult = await isOwnerTask;

        var response = new RecipeAPIResponse
        {
            Policy = new RecipePolicy
            {
                IsAFavorite = isFavoriteResult.Item2,
                CanEdit = canEditResult.Item2,
                IsOwner = isOwnerResult.Item2
            },
            Data = getDataResult
        };
        return Ok(response);
    }

    [HttpPost("{recipeId:guid}/tags")]
    public async Task<ActionResult> CreateRecipeTag([FromRoute] Guid recipeId, [FromBody] RecipeTagDto tag)
    {
        var user = _currentUserService.UserId;
        var canEditResult = await CanUserEditRecipe(recipeId, user);
        if (!canEditResult.Item2) return Forbid();
        var createdRecipeTag = await _cookBookService.CreateRecipeTagAsync(recipeId, tag);
        return CreatedAtAction(
            nameof(GetRecipeTag),
            new {recipeId, tagId = tag.TagId},
            createdRecipeTag
        );
    }

    [HttpPut("{recipeId:guid}/tags")]
    public async Task<ActionResult> BatchUpdateRecipeTags([FromRoute] Guid recipeId, [FromBody] List<RecipeTagDto> tags)
    {
        var user = _currentUserService.UserId;
        var canEditResult = await CanUserEditRecipe(recipeId, user);
        if (!canEditResult.Item2) return Forbid();
        foreach (var tag in tags)
        {
            tag.CreatedBy = user;
        }
        await _cookBookService.BatchUpdateRecipeTagsAsync(recipeId, tags);
        return Ok();
    }

    [HttpGet("{recipeId:guid}/tags/{tagId:guid}")]
    public async Task<ActionResult> GetRecipeTag([FromRoute] Guid recipeId, [FromRoute] Guid tagId)
    {
        var recipeTag = await _cookBookService.GetRecipeTagAsync(recipeId, tagId);
        return Ok(new { recipeTag.TagId, recipeTag.TagName });
    }


    [HttpGet("{recipeId:guid}/similar-recipes")]
    public async Task<ActionResult> GetSimilarRecipes([FromRoute] Guid recipeId,
        [FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        var userId = _currentUserService.UserId;
        var similarRecipes = await _analyticsService.GetSimilarRecipesAsync(recipeId, page, limit);
        similarRecipes.Items ??= new List<RecipeSimilarityDto>();
        var similarRecipeIds = similarRecipes.Items.Select(x => x.SimilarRecipeId).ToList();

        var getFavoritesTask = AreRecipesFavoritesOfTheUser(similarRecipeIds, userId);
        var getRecipesTask = _cookBookService.GetRecipesAsync(similarRecipeIds, 1, limit);

        var recipes = await getRecipesTask;
        recipes.Items ??= new List<RecipesDto>();
        var recipesDict = recipes.Items.ToDictionary(x => x.Id, x => x);
        var favoritesDict = await getFavoritesTask;

        var items = similarRecipes.Items.Select(x => new
        {
            Id = x.SimilarRecipeId,
            recipesDict[x.SimilarRecipeId].Name,
            recipesDict[x.SimilarRecipeId].EstimatedMinutes,
            recipesDict[x.SimilarRecipeId].Serves,
            recipesDict[x.SimilarRecipeId].Yield,
            Images = recipesDict[x.SimilarRecipeId].Images?.Select(i => new { i.ImageId, i.Url }).ToList() ?? null,
            IsAFavorite = favoritesDict[x.RecipeId],
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
            UserId = userId,
            CreatedBy = userId
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

    private async Task<(Guid, bool)> IsRecipeAFavoriteOfTheUser(Guid recipeId, string userId)
    {
        try
        {
            var recipeIds = new List<Guid>(new[] { recipeId });
            var favorite = await _usersService.GetUsersFavoritesAsync(userId, recipeIds, 1, 1);
            var isAFavorite = favorite.Items?.Count > 0;
            return (recipeId, isAFavorite);
        }
        catch 
        {
            return (recipeId, false);
        }
    }

    private async Task<Dictionary<Guid, bool>> AreRecipesFavoritesOfTheUser(List<Guid> recipeIds, string userId)
    {
        var favoritesDict = recipeIds
            .ToDictionary(x => x, x => false);
        try
        {
            var recipeFavorites = await _usersService.GetUsersFavoritesAsync(
                userId, recipeIds, 1, recipeIds.Count);
            if (recipeFavorites.Items is { Count: > 0 })
            {
                foreach (var favoriteRecipeId in recipeFavorites.Items)
                {
                    if (favoritesDict.ContainsKey(favoriteRecipeId.RecipeId))
                    {
                        favoritesDict[favoriteRecipeId.RecipeId] = true;
                    }
                }
            }
        }
        catch { }
        return favoritesDict;
    }

    private async Task<Dictionary<Guid, bool>> CanUserEditRecipes(List<Guid> recipeIds, string userId)
    {
        var editableRecipesDict = recipeIds
            .ToDictionary(x => x, x => false);
        try
        {
            var recipeEntitlements = await _usersService.GetRecipeEntitlementsAsync(
                recipeIds, userId);
            if (recipeEntitlements.Items is { Count: > 0 })
            {
                foreach (var recipeEntitlement in recipeEntitlements.Items)
                {
                    if (editableRecipesDict.ContainsKey(recipeEntitlement.RecipeId))
                    {
                        editableRecipesDict[recipeEntitlement.RecipeId] = recipeEntitlement.Type.Contains("author")
                            || recipeEntitlement.Type.Contains("contributor");
                    }
                }
            }
        }
        catch { }
        return editableRecipesDict;
    }

    private async Task<(Guid, bool)> CanUserEditRecipe(Guid recipeId, string userId)
    {
        var canEdit = false;
        try
        {
            var recipeEntitlements = await _usersService.GetRecipeEntitlementsAsync(recipeId, userId);
            if (recipeEntitlements.Items?.Count > 0)
            {
                canEdit = recipeEntitlements.Items.Select(x => x.Type).Contains("author")
                    || recipeEntitlements.Items.Select(x => x.Type).Contains("contributor");
            }
        }
        catch { }
        return (recipeId, canEdit);
    }


    private async Task<Dictionary<Guid, bool>> IsUserOwnerOfTheRecipes(List<Guid> recipeIds, string userId)
    {
        var recipeOwnersDict = recipeIds
            .ToDictionary(x => x, x => false);
        try
        {
            var recipeEntitlements = await _usersService.GetRecipeEntitlementsAsync(recipeIds, userId);
            if (recipeEntitlements.Items is { Count: > 0 })
            {
                foreach (var recipeEntitlement in recipeEntitlements.Items)
                {
                    if (recipeOwnersDict.ContainsKey(recipeEntitlement.RecipeId))
                    {
                        recipeOwnersDict[recipeEntitlement.RecipeId] = recipeEntitlement.Type.Contains("author");
                    }
                }
            }
        }
        catch { }
        return recipeOwnersDict;
    }

    private async Task<(Guid, bool)> IsUserOwnerOfTheRecipe(Guid recipeId, string userId)
    {
        var isOwner = false;
        try
        {
            var recipeEntitlements = await _usersService.GetRecipeEntitlementsAsync(recipeId, userId);
            if (recipeEntitlements.Items?.Count > 0)
            {
                isOwner = recipeEntitlements.Items.Select(x => x.Type).Contains("author");
            }
        }
        catch { }
        return (recipeId, isOwner);
    }
}