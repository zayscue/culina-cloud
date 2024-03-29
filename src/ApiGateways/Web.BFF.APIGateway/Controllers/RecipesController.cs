﻿namespace CulinaCloud.Web.BFF.APIGateway.Controllers;

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

    private async Task<(Guid, RecipePolicy)> GetRecipePolicy(string userId, Guid recipeId)
    {
        var recipeIds = new List<Guid>
        {
            recipeId
        };
        var applicationUserPolicies = await _usersService.GetApplicationUserPoliciesAsync(userId, recipeIds);
        var applicationUserPolicy = applicationUserPolicies.FirstOrDefault();
        if (applicationUserPolicy == null)
        {
            return (recipeId, new RecipePolicy
            {
                IsAFavorite = false,
                IsOwner = false,
                CanEdit = false,
                CanShare = false
            });
        }
        return (applicationUserPolicy.RecipeId, new RecipePolicy
        {
            IsOwner = applicationUserPolicy.IsOwner,
            IsAFavorite = applicationUserPolicy.IsAFavorite,
            CanEdit = applicationUserPolicy.CanEdit,
            CanShare = applicationUserPolicy.CanShare
        });
    }

    private async Task<Dictionary<Guid, RecipePolicy>> GetRecipePolicies(string userId, List<Guid> recipeIds)
    {
        var recipePoliciesDict = recipeIds.ToDictionary(x => x, x => new RecipePolicy
        {
            IsAFavorite = false,
            IsOwner = false,
            CanEdit = false,
            CanShare = false
        });

        var applicationUserPolicies = await _usersService.GetApplicationUserPoliciesAsync(userId, recipeIds);
        if (applicationUserPolicies != null && applicationUserPolicies.Count > 0)
        {
            foreach (var applicationUserPolicy in applicationUserPolicies)
            {
                if (recipePoliciesDict.ContainsKey(applicationUserPolicy.RecipeId))
                {
                    var recipePolicy = new RecipePolicy
                    {
                        IsOwner = applicationUserPolicy.IsOwner,
                        IsAFavorite = applicationUserPolicy.IsAFavorite,
                        CanEdit = applicationUserPolicy.CanEdit,
                        CanShare = applicationUserPolicy.CanShare
                    };
                    recipePoliciesDict[applicationUserPolicy.RecipeId] = recipePolicy;
                }
            }
        }
        return recipePoliciesDict;
    }


    private async Task<bool> CanUserEditRecipe(string userId, Guid recipeId)
    {
        var getRecipePolicyTask = GetRecipePolicy(userId, recipeId);
        var recipePolicy = await getRecipePolicyTask;
        return recipePolicy.Item2.CanEdit;
    }

    private bool CanUserEditRecipe(RecipePolicy policy)
    {
        return policy.CanEdit;
    }

    private async Task<bool> CanUserShareRecipe(string userId, Guid recipeId)
    {
        var getRecipePolicyTask = GetRecipePolicy(userId, recipeId);
        var recipePolicy = await getRecipePolicyTask;
        return recipePolicy.Item2.CanShare;
    }

    private bool CanUserShareRecipe(RecipePolicy policy)
    {
        return policy.CanShare;
    }

    private async Task<bool> IsUserTheRecipeOwner(string userId, Guid recipeId)
    {
        var getRecipePolicyTask = GetRecipePolicy(userId, recipeId);
        var recipePolicy = await getRecipePolicyTask;
        return recipePolicy.Item2.IsOwner;
    }

    private bool IsUserTheRecipeOwner(RecipePolicy policy)
    {
        return policy.IsOwner;
    }

    private async Task<bool> DoesRecipeExist(Guid recipeId)
    {
        try
        {
            var recipe = await _cookBookService.GetRecipeAsync(recipeId);
            return recipe != null;
        }
        catch(RecipeNotFoundException)
        {
            return false;
        }
    }

    [HttpGet("recommended")]
    [Authorize("read:recommended_recipes")]
    public async Task<ActionResult> GetPersonalRecipeFeed([FromQuery] int page = 1, [FromQuery] int limit = 100)
    {
        var userId = _currentUserService.UserId;
        var recipeRecommendations = await _analyticsService.GetPersonalizedRecipeRecommendationsAsync(
            userId, page, limit);
        recipeRecommendations.Items ??= new List<RecipeRecommendationDto>();
        var recipeIds = recipeRecommendations.Items.Select(x => x.RecipeId).ToList();

        var getRecipePoliciesTask = GetRecipePolicies(userId, recipeIds);
        var getRecipesTask = _cookBookService.GetRecipesAsync(recipeIds, 1, limit);
        var getRecipePopularitiesTask = _analyticsService.GetRecipePopularitiesAsync(recipeIds);

        var recipes = await getRecipesTask;
        var recipesDict = recipes.Items?
            .ToDictionary(x => x.Id, x => x) ?? new Dictionary<Guid, RecipesDto>();


        var recipePoliciesDict = await getRecipePoliciesTask;
        var recipePopularities = await getRecipePopularitiesTask;
        var recipePopularitiesDict = recipePopularities.Items?.ToDictionary(x => x.RecipeId, x => x)
                                     ?? new Dictionary<Guid, RecipePopularityDto>();

        var feedItems = recipeRecommendations.Items.Select(x =>
            new RecipeAPIResponse
            {
                Policy = recipePoliciesDict[x.RecipeId],
                Popularity = recipePopularitiesDict.ContainsKey(x.RecipeId)
                    ? recipePopularitiesDict[x.RecipeId]
                    : null,
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
    [Authorize("read:favorite_recipes")]
    public async Task<ActionResult> GetFavoriteRecipes([FromQuery] int page = 1, [FromQuery] int limit = 100)
    {
        var userId = _currentUserService.UserId;
        var favoriteRecipes = await _usersService.GetUsersFavoritesAsync(
            userId, page, limit);
        favoriteRecipes.Items ??= new List<FavoriteDto>();
        var recipeIds = favoriteRecipes.Items.Select(x => x.RecipeId).ToList();

        var getRecipePoliciesTask = GetRecipePolicies(userId, recipeIds);
        var getRecipesTask = _cookBookService.GetRecipesAsync(recipeIds, 1, limit);
        var getRecipePopularitiesTask = _analyticsService.GetRecipePopularitiesAsync(recipeIds);

        var recipes = await getRecipesTask;
        var recipePoliciesDict = await getRecipePoliciesTask;
        var recipePopularities = await getRecipePopularitiesTask;
        var recipePopularitiesDict = recipePopularities.Items?.ToDictionary(x => x.RecipeId, x => x) 
                                     ?? new Dictionary<Guid, RecipePopularityDto>();


        var items = recipes.Items?.Select(x =>
            new RecipeAPIResponse
            {
                Policy = recipePoliciesDict[x.Id],
                Popularity = recipePopularitiesDict.ContainsKey(x.Id)
                    ? recipePopularitiesDict[x.Id]
                    : null,
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

    [HttpGet("mine")]
    [Authorize("read:my_recipes")]
    public async Task<ActionResult> GetMyRecipes([FromQuery] int page = 1, [FromQuery] int limit = 100)
    {
        var userId = _currentUserService.UserId;
        var recipeEntitlements = await _usersService.GetRecipeEntitlementsAsync(userId, page, limit);
        recipeEntitlements.Items ??= new List<RecipeEntitlementDto>();
        var recipeIds = recipeEntitlements.Items.Select(x => x.RecipeId).ToList();

        var getRecipePoliciesTask = GetRecipePolicies(userId, recipeIds);
        var getRecipesTask = _cookBookService.GetRecipesAsync(recipeIds, 1, limit);
        var getRecipePopularitiesTask = _analyticsService.GetRecipePopularitiesAsync(recipeIds);

        var recipes = await getRecipesTask;
        var recipePoliciesDict = await getRecipePoliciesTask;
        var recipePopularities = await getRecipePopularitiesTask;
        var recipePopularitiesDict = recipePopularities.Items?.ToDictionary(x => x.RecipeId, x => x)
                                     ?? new Dictionary<Guid, RecipePopularityDto>();

        var items = recipes.Items?.Select(x =>
            new RecipeAPIResponse
            {
                Policy = recipePoliciesDict[x.Id],
                Popularity = recipePopularitiesDict.ContainsKey(x.Id)
                    ? recipePopularitiesDict[x.Id]
                    : null,
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
            Page = recipeEntitlements.Page,
            TotalCount = recipeEntitlements.TotalCount,
            TotalPages = recipeEntitlements.TotalPages
        });
    }

    [HttpGet("popular")]
    [Authorize("read:popular_recipes")]
    public async Task<ActionResult> GetPopularRecipes([FromQuery] int page = 1, [FromQuery] int limit = 100)
    {
        var userId = _currentUserService.UserId;
        var popularRecipes = await _analyticsService.GetPopularRecipesAsync(page, limit, "ratingWeightedAverage", true);
        popularRecipes.Items ??= new List<RecipePopularityDto>();
        var recipeIds = popularRecipes.Items.Select(x => x.RecipeId).ToList();

        var getRecipePoliciesTask = GetRecipePolicies(userId, recipeIds);
        var getRecipesTask = _cookBookService.GetRecipesAsync(recipeIds, 1, limit);
        var getRecipePopularitiesTask = _analyticsService.GetRecipePopularitiesAsync(recipeIds);

        var recipes = await getRecipesTask;
        recipes.Items ??= new List<RecipesDto>();
        var recipesDict = recipes.Items.ToDictionary(x => x.Id, x => x);
        var recipePoliciesDict = await getRecipePoliciesTask;
        var recipePopularities = await getRecipePopularitiesTask;
        var recipePopularitiesDict = recipePopularities.Items?.ToDictionary(x => x.RecipeId, x => x)
                                     ?? new Dictionary<Guid, RecipePopularityDto>();

        var items = popularRecipes.Items.Select(x =>
            new RecipeAPIResponse
            {
                Policy = recipePoliciesDict[x.RecipeId],
                Popularity = recipePopularitiesDict.ContainsKey(x.RecipeId)
                    ? recipePopularitiesDict[x.RecipeId]
                    : null,
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

    [HttpGet("recent")]
    [Authorize("read:recent_recipes")]
    public async Task<ActionResult> GetRecentRecipes([FromQuery] int page = 1, [FromQuery] int limit = 100)
    {
        var userId = _currentUserService.UserId;
        var recentRecipes = await _analyticsService.GetRecentRecipesAsync(page, limit);
        recentRecipes.Items ??= new List<RecentRecipeDto>();
        var recipeIds = recentRecipes.Items.Select(x => x.RecipeId).ToList();

        var getRecipePoliciesTask = GetRecipePolicies(userId, recipeIds);
        var getRecipesTask = _cookBookService.GetRecipesAsync(recipeIds, 1, limit);
        var getRecipePopularitiesTask = _analyticsService.GetRecipePopularitiesAsync(recipeIds);

        var recipes = await getRecipesTask;
        recipes.Items ??= new List<RecipesDto>();
        var recipesDict = recipes.Items.ToDictionary(x => x.Id, x => x);
        var recipePoliciesDict = await getRecipePoliciesTask;
        var recipePopularities = await getRecipePopularitiesTask;
        var recipePopularitiesDict = recipePopularities.Items?.ToDictionary(x => x.RecipeId, x => x)
                                     ?? new Dictionary<Guid, RecipePopularityDto>();

        var items = recentRecipes.Items.Select(x =>
            new RecipeAPIResponse
            {
                Policy = recipePoliciesDict[x.RecipeId],
                Popularity = recipePopularitiesDict.ContainsKey(x.RecipeId)
                    ? recipePopularitiesDict[x.RecipeId]
                    : null,
                Data = new
                {
                    Id = x.RecipeId,
                    recipesDict[x.RecipeId].Name,
                    recipesDict[x.RecipeId].EstimatedMinutes,
                    recipesDict[x.RecipeId].Serves,
                    recipesDict[x.RecipeId].Yield,
                    Images = recipesDict[x.RecipeId].Images?.Select(i => new { i.ImageId, i.Url }).ToList() ?? null,
                    x.Submitted,
                    x.PopularityScore
                }
            }
        ).ToList();
        return Ok(new PaginatedRecipeAPIResponse
        {
            Items = items,
            Page = recentRecipes.Page,
            TotalCount = recentRecipes.TotalCount,
            TotalPages = recentRecipes.TotalPages
        });
    }

    [HttpGet]
    [Authorize("read:recipes")]
    public async Task<ActionResult> GetRecipes([FromQuery] string name = "", [FromQuery] int page = 1, [FromQuery] int limit = 100)
    {
        var userId = _currentUserService.UserId;
        var recipes = await _cookBookService.GetRecipesAsync(name, page, limit);
        recipes.Items ??= new List<RecipesDto>();
        var getRecipePoliciesTask = GetRecipePolicies(userId, recipes.Items.Select(x => x.Id).ToList());
        var recipePoliciesDict = await getRecipePoliciesTask;

        var items = recipes.Items.Select(x =>
            new RecipeAPIResponse
            {
                Policy = recipePoliciesDict[x.Id],
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
            Page = recipes.Page,
            TotalCount = recipes.TotalCount,
            TotalPages = recipes.TotalPages
        });
    }

    [HttpPost]
    [Authorize("create:recipe")]
    public async Task<ActionResult> CreateRecipe([FromBody] CreateRecipeDto recipe)
    {
        try
        {
            var user = _currentUserService.UserId;
            recipe.CreatedBy = user;
            var createdRecipe = await _cookBookService.CreateRecipeAsync(recipe);

            var cancellationToken = new CancellationToken();
            var createRecipeEntitlementTask = _usersService.CreateRecipeEntitlementAsync(new RecipeEntitlementDto
            {
                RecipeId = createdRecipe.Id,
                UserId = user,
                GrantedBy = user,
                Type = "author"
            }, cancellationToken);
            var createRecipePopularityStatTask = _analyticsService.CreateRecipePopularityStatAsync(createdRecipe.Id, cancellationToken);
            var createdRecipeEntitlement = await createRecipeEntitlementTask;
            var createRecipePopularityStat = await createRecipePopularityStatTask;

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
                        IsOwner = createdRecipeEntitlement != null,
                        CanShare = createdRecipeEntitlement != null
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
                        Ingredients = createdRecipe.Ingredients?.Select(x => new
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
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpGet("{recipeId:guid}")]
    [Authorize("read:recipe")]
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
        var getRecipePolicyTask = GetRecipePolicy(userId, recipeId);
        var getRecipePopularityTask = _analyticsService.GetRecipePopularityAsync(recipeId);

        var getDataResult = await getDataTask;
        var getRecipePolicyResult = await getRecipePolicyTask;
        var getRecipePopularityResult = await getRecipePopularityTask;

        var response = new RecipeAPIResponse
        {
            Policy = getRecipePolicyResult.Item2,
            Popularity = getRecipePopularityResult,
            Data = getDataResult
        };
        return Ok(response);
    }

    [HttpPut("{recipeId:guid}")]
    [Authorize("update:recipe")]
    public async Task<ActionResult> UpdateRecipe([FromRoute] Guid recipeId, [FromBody] RecipeDto recipe)
    {
        var user = _currentUserService.UserId;
        var canUserEditRecipe = await CanUserEditRecipe(user, recipeId);
        if (!canUserEditRecipe) return Forbid();

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
    [Authorize("create:recipe_nutrition")]
    public async Task<ActionResult> CreateRecipeNutrition([FromRoute] Guid recipeId,
        [FromBody] RecipeNutritionDto nutrition)
    {
        var user = _currentUserService.UserId;
        var getRecipePolicy = await GetRecipePolicy(user, recipeId);
        var canUserEditRecipe = CanUserEditRecipe(getRecipePolicy.Item2);
        if (!canUserEditRecipe) return Forbid();
        nutrition.CreatedBy = user;
        nutrition.RecipeId = recipeId;
        var createdNutrition = await _cookBookService.CreateRecipeNutritionAsync(recipeId, nutrition);
        return CreatedAtAction(
            nameof(GetRecipeNutrition),
            new { },
            new RecipeAPIResponse
            {
                Policy = getRecipePolicy.Item2,
                Data = createdNutrition
            }
        );
    }

    [HttpGet("{recipeId:guid}/nutrition")]
    [Authorize("read:recipe_nutrition")]
    public async Task<ActionResult> GetRecipeNutrition([FromRoute] Guid recipeId)
    {
        async Task<object?> GetData(Guid id)
        {
            var nutrition = await _cookBookService.GetRecipeNutritionAsync(id);
            return nutrition;
        };
        var userId = _currentUserService.UserId;
        var getDataTask = GetData(recipeId);
        var getRecipePolicyTask = GetRecipePolicy(userId, recipeId);

        var getDataResult = await getDataTask;
        var getRecipePolicyResult = await getRecipePolicyTask;

        var response = new RecipeAPIResponse
        {
            Policy = getRecipePolicyResult.Item2,
            Data = getDataResult
        };
        return Ok(response);
    }

    [HttpPut("{recipeId:guid}/nutrition")]
    [Authorize("update:recipe_nutrition")]
    public async Task<ActionResult> UpdateRecipeNutrition([FromRoute] Guid recipeId,
        [FromBody] RecipeNutritionDto nutrition)
    {
        var user = _currentUserService.UserId;
        var canUserEditRecipe = await CanUserEditRecipe(user, recipeId);
        if (!canUserEditRecipe) return Forbid();
        nutrition.RecipeId = recipeId;
        nutrition.LastModifiedBy = user;
        await _cookBookService.UpdateRecipeNutritionAsync(recipeId, nutrition);
        return Ok();
    }

    [HttpGet("{recipeId:guid}/steps")]
    [Authorize("read:recipe_steps")]
    public async Task<ActionResult> GetRecipeSteps([FromRoute] Guid recipeId)
    {
        var userId = _currentUserService.UserId;
;
        var getDataTask = _cookBookService.GetRecipeStepsAsync(recipeId);
        var getRecipePolicyTask = GetRecipePolicy(userId, recipeId);

        var getDataResult = await getDataTask;
        var getRecipePolicyResult = await getRecipePolicyTask;

        return Ok(getDataResult.Select(x => new RecipeAPIResponse
        {
            Data = x,
            Policy = getRecipePolicyResult.Item2
        }));
    }

    [HttpGet("{recipeId:guid}/steps/{order:int}")]
    [Authorize("read:recipe_step")]
    public async Task<ActionResult> GetRecipeStep([FromRoute] Guid recipeId, [FromRoute] int order)
    {
        var userId = _currentUserService.UserId;
        async Task<object?> GetData(Guid id, int ix)
        {
            var recipeStep = await _cookBookService.GetRecipeStepAsync(id, ix);
            return recipeStep;
        };
        var getDataTask = GetData(recipeId, order);
        var getRecipePolicyTask = GetRecipePolicy(userId, recipeId);

        var getDataResult = await getDataTask;
        var getRecipePolicyResult = await getRecipePolicyTask;

        var response = new RecipeAPIResponse
        {
            Policy = getRecipePolicyResult.Item2,
            Data = getDataResult
        };
        return Ok(response);
    }

    [HttpPut("{recipeId:guid}/steps")]
    [Authorize("update:recipe_steps")]
    public async Task<ActionResult> BatchUpdateRecipeSteps([FromRoute] Guid recipeId,
        [FromBody] List<RecipeStepDto> steps)
    {
        var user = _currentUserService.UserId;
        var canUserEditRecipe = await CanUserEditRecipe(user, recipeId);
        if (!canUserEditRecipe) return Forbid();
        foreach (var step in steps)
        {
            step.RecipeId = recipeId;
            step.CreatedBy = user;
        }
        await _cookBookService.BatchUpdateRecipeStepsAsync(recipeId, steps);
        return Ok();
    }

    [HttpGet("{recipeId:guid}/ingredients")]
    [Authorize("read:recipe_ingredients")]
    public async Task<ActionResult> GetRecipeIngredients([FromRoute] Guid recipeId)
    {
        var userId = _currentUserService.UserId;
        var getDataTask = _cookBookService.GetRecipeIngredientsAsync(recipeId);
        var getRecipePolicyTask = GetRecipePolicy(userId, recipeId);

        var getDataResult = await getDataTask;
        var getRecipePolicyResult = await getRecipePolicyTask;

        return Ok(getDataResult.Select(x => new RecipeAPIResponse
        {
            Data = x,
            Policy = getRecipePolicyResult.Item2
        }));
    }

    [HttpPost("{recipeId:guid}/ingredients")]
    [Authorize("create:recipe_ingredient")]
    public async Task<ActionResult> CreateRecipeIngredient([FromRoute] Guid recipeId,
        [FromBody] RecipeIngredientDto recipeIngredient)
    {
        var user = _currentUserService.UserId;
        var getRecipePolicy = await GetRecipePolicy(user, recipeId);
        var canUserEditRecipe = CanUserEditRecipe(getRecipePolicy.Item2);
        if (!canUserEditRecipe) return Forbid();
        recipeIngredient.CreatedBy = user;
        recipeIngredient.RecipeId = recipeId;
        var createdRecipeIngredient = await _cookBookService.CreateRecipeIngredientAsync(recipeId, recipeIngredient);
        return CreatedAtAction(
            nameof(GetRecipeIngredient),
            new {recipeId, recipeIngredientId = createdRecipeIngredient.Id},
            new RecipeAPIResponse
            {
                Policy = getRecipePolicy.Item2,
                Data = new
                {
                    createdRecipeIngredient.Id,
                    createdRecipeIngredient.Quantity,
                    createdRecipeIngredient.Part,
                    createdRecipeIngredient.IngredientId,
                    createdRecipeIngredient.IngredientName
                }
            });
    }

    [HttpPut("{recipeId:guid}/ingredients")]
    [Authorize("update:recipe_ingredients")]
    public async Task<ActionResult> BatchUpdateRecipeIngredients([FromRoute] Guid recipeId,
        [FromBody] List<RecipeIngredientDto> ingredients)
    {
        var user = _currentUserService.UserId;
        var canUserEditRecipe = await CanUserEditRecipe(user, recipeId);
        if (!canUserEditRecipe) return Forbid();
        foreach (var ingredient in ingredients)
        {
            ingredient.RecipeId = recipeId;
            ingredient.CreatedBy = user;
        }
        await _cookBookService.BatchUpdateRecipeIngredientsAsync(recipeId, ingredients);
        return Ok();
    }

    [HttpGet("{recipeId:guid}/ingredients/{recipeIngredientId:guid}")]
    [Authorize("read:recipe_ingredient")]
    public async Task<ActionResult> GetRecipeIngredient([FromRoute] Guid recipeId, [FromRoute] Guid recipeIngredientId)
    {
        var userId = _currentUserService.UserId;
        async Task<object?> GetData(Guid _recipeId, Guid _recipeIngredientId)
        {
            var recipeIngredient = await _cookBookService.GetRecipeIngredientAsync(_recipeId, _recipeIngredientId);
            return recipeIngredient;
        };
        var getDataTask = GetData(recipeId, recipeIngredientId);
        var getRecipePolicyTask = GetRecipePolicy(userId, recipeId);

        var getDataResult = await getDataTask;
        var getRecipePolicyResult = await getRecipePolicyTask;

        var response = new RecipeAPIResponse
        {
            Policy = getRecipePolicyResult.Item2,
            Data = getDataResult
        };
        return Ok(response);
    }

    [HttpGet("{recipeId:guid}/images")]
    [Authorize("read:recipe_images")]
    public async Task<ActionResult> GetRecipeImages([FromRoute] Guid recipeId)
    {
        var userId = _currentUserService.UserId;
        var getDataTask = _cookBookService.GetRecipeImagesAsync(recipeId);
        var getRecipePolicyTask = GetRecipePolicy(userId, recipeId);

        var getDataResult = await getDataTask;
        var getRecipePolicyResult = await getRecipePolicyTask;

        return Ok(getDataResult.Select(x => new RecipeAPIResponse
        {
            Data = x,
            Policy = getRecipePolicyResult.Item2
        }));
    }

    [HttpPost("{recipeId:guid}/images")]
    [Authorize("create:recipe_image")]
    public async Task<ActionResult> CreateRecipeImage([FromRoute] Guid recipeId, [FromBody] RecipeImageDto image)
    {
        var user = _currentUserService.UserId;
        var getRecipePolicy = await GetRecipePolicy(user, recipeId);
        var canUserEditRecipe = CanUserEditRecipe(getRecipePolicy.Item2);
        if (!canUserEditRecipe) return Forbid();
        image.CreatedBy = user;
        var createdRecipeImage = await _cookBookService.CreateRecipeImageAsync(recipeId, image);
        return CreatedAtAction(
            nameof(GetRecipeImage),
            new {recipeId, imageId = image.ImageId},
            new RecipeAPIResponse
            {
                Policy = getRecipePolicy.Item2,
                Data = createdRecipeImage
            });
    }

    [HttpPut("{recipeId:guid}/images")]
    [Authorize("update:recipe_images")]
    public async Task<ActionResult> BatchUpdateRecipeImages([FromRoute] Guid recipeId,
        [FromBody] List<RecipeImageDto> images)
    {
        var user = _currentUserService.UserId;
        var canUserEditRecipe = await CanUserEditRecipe(user, recipeId);
        if (!canUserEditRecipe) return Forbid();
        foreach (var image in images)
        {
            image.CreatedBy = user;
        }
        await _cookBookService.BatchUpdateRecipeImagesAsync(recipeId, images);
        return Ok();
    }

    [HttpGet("{recipeId:guid}/images/{imageId:guid}")]
    [Authorize("read:recipe_image")]
    public async Task<ActionResult> GetRecipeImage([FromRoute] Guid recipeId, [FromRoute] Guid imageId)
    {
        var userId = _currentUserService.UserId;
        async Task<object?> GetData(Guid _recipeId, Guid _imageId)
        {
            var recipeImage = await _cookBookService.GetRecipeImageAsync(_recipeId, _imageId);
            return recipeImage;
        };
        var getDataTask = GetData(recipeId, imageId);
        var getRecipePolicyTask = GetRecipePolicy(userId, recipeId);

        var getDataResult = await getDataTask;
        var getRecipePolicyResult = await getRecipePolicyTask;

        var response = new RecipeAPIResponse
        {
            Policy = getRecipePolicyResult.Item2,
            Data = getDataResult
        };
        return Ok(response);
    }

    [HttpGet("{recipeId:guid}/tags")]
    [Authorize("read:recipe_tags")]
    public async Task<ActionResult> GetRecipeTags([FromRoute] Guid recipeId)
    {
        var userId = _currentUserService.UserId;
        var getDataTask = _cookBookService.GetRecipeTagsAsync(recipeId);
        var getRecipePolicyTask = GetRecipePolicy(userId, recipeId);

        var getDataResult = await getDataTask;
        var getRecipePolicyResult = await getRecipePolicyTask;

        return Ok(getDataResult.Select(x => new RecipeAPIResponse
        {
            Data = x,
            Policy = getRecipePolicyResult.Item2
        }));
    }

    [HttpPost("{recipeId:guid}/tags")]
    [Authorize("create:recipe_tag")]
    public async Task<ActionResult> CreateRecipeTag([FromRoute] Guid recipeId, [FromBody] RecipeTagDto tag)
    {
        var user = _currentUserService.UserId;
        var getRecipePolicy = await GetRecipePolicy(user, recipeId);
        var canUserEditRecipe = CanUserEditRecipe(getRecipePolicy.Item2);
        if (!canUserEditRecipe) return Forbid();
        var createdRecipeTag = await _cookBookService.CreateRecipeTagAsync(recipeId, tag);
        return CreatedAtAction(
            nameof(GetRecipeTag),
            new {recipeId, tagId = tag.TagId},
            new RecipeAPIResponse
            {
                Policy = getRecipePolicy.Item2,
                Data = createdRecipeTag
            });
    }

    [HttpPut("{recipeId:guid}/tags")]
    [Authorize("update:recipe_tags")]
    public async Task<ActionResult> BatchUpdateRecipeTags([FromRoute] Guid recipeId, [FromBody] List<RecipeTagDto> tags)
    {
        var user = _currentUserService.UserId;
        var canUserEditRecipe = await CanUserEditRecipe(user, recipeId);
        if (!canUserEditRecipe) return Forbid();
        foreach (var tag in tags)
        {
            tag.CreatedBy = user;
        }
        await _cookBookService.BatchUpdateRecipeTagsAsync(recipeId, tags);
        return Ok();
    }

    [HttpGet("{recipeId:guid}/tags/{tagId:guid}")]
    [Authorize("read:recipe_tag")]
    public async Task<ActionResult> GetRecipeTag([FromRoute] Guid recipeId, [FromRoute] Guid tagId)
    {
        var userId = _currentUserService.UserId;
        var getRecipePolicyTask = await GetRecipePolicy(userId, recipeId);
        var recipeTag = await _cookBookService.GetRecipeTagAsync(recipeId, tagId);
        return Ok(new RecipeAPIResponse
        {
            Policy = getRecipePolicyTask.Item2,
            Data = new
            {
                recipeTag.TagId,
                recipeTag.TagName
            }
        });
    }


    [HttpGet("{recipeId:guid}/similar")]
    [Authorize("read:similar_recipes")]
    public async Task<ActionResult> GetSimilarRecipes([FromRoute] Guid recipeId,
        [FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        var userId = _currentUserService.UserId;
        var similarRecipes = await _analyticsService.GetSimilarRecipesAsync(recipeId, page, limit);
        similarRecipes.Items ??= new List<RecipeSimilarityDto>();
        var recipeIds = similarRecipes.Items.Select(x => x.SimilarRecipeId).ToList();

        var getRecipePoliciesTask = GetRecipePolicies(userId, recipeIds);
        var getRecipesTask = _cookBookService.GetRecipesAsync(recipeIds, 1, limit);
        var getRecipePopularitiesTask = _analyticsService.GetRecipePopularitiesAsync(recipeIds);

        var recipes = await getRecipesTask;
        recipes.Items ??= new List<RecipesDto>();
        var recipesDict = recipes.Items.ToDictionary(x => x.Id, x => x);
        var recipePoliciesDict = await getRecipePoliciesTask;
        var recipePopularities = await getRecipePopularitiesTask;
        var recipePopularitiesDict = recipePopularities.Items?.ToDictionary(x => x.RecipeId, x => x)
                                     ?? new Dictionary<Guid, RecipePopularityDto>();

        var items = similarRecipes.Items.Select(x =>
            new RecipeAPIResponse
            {
                Policy = recipePoliciesDict[x.SimilarRecipeId],
                Popularity = recipePopularitiesDict.ContainsKey(x.SimilarRecipeId) 
                    ? recipePopularitiesDict[x.SimilarRecipeId] 
                    : null,
                Data = new
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
                }
            }
        ).ToList();
        return Ok(new PaginatedRecipeAPIResponse
        {
            Items = items,
            Page = similarRecipes.Page,
            TotalCount = similarRecipes.TotalCount,
            TotalPages = similarRecipes.TotalPages
        });
    }

    [HttpGet("{recipeId:guid}/reviews")]
    [Authorize("read:recipe_reviews")]
    public async Task<ActionResult> GetRecipeReviews([FromRoute] Guid recipeId, [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        var recipeReviews = await _interactionsService.GetRecipeReviews(recipeId, page, limit);
        return Ok(recipeReviews);
    }

    [HttpPost("{recipeId:guid}/reviews")]
    [Authorize("create:recipe_review")]
    public async Task<ActionResult> CreateRecipeReview([FromRoute] Guid recipeId, [FromBody] ReviewDto review)
    {
        var user = _currentUserService.UserId;
        review.RecipeId = recipeId;
        review.CreatedBy = user;
        review.UserId = user;

        var doesRecipeExist = await DoesRecipeExist(review.RecipeId);
        if (!doesRecipeExist)
        {
            return BadRequest($"Recipe {review.RecipeId} Not Found");
        }

        try
        {
            var createdReview = await _interactionsService.CreateRecipeReviewAsync(review);
            await _analyticsService.UpdateRecipePopularityStatAsync(recipeId, review.Rating);
            return Ok(createdReview);
        }
        catch (ReviewAlreadyExistsException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{recipeId:guid}/share")]
    [Authorize("read:recipe_entitlements")]
    public async Task<ActionResult> GetRecipeShares([FromRoute] Guid recipeId, [FromQuery] int page = 1, [FromQuery] int limit = 100)
    {
        var userId = _currentUserService.UserId;
        var isOwner = await IsUserTheRecipeOwner(userId, recipeId);
        if (!isOwner)
        {
            Forbid();
        }

        var recipeEntitlements = await _usersService.GetRecipeEntitlementsAsync(recipeId, page, limit);
        var userIds = recipeEntitlements.Items?.Select(x => x.UserId).ToList() ?? new List<string>();
        var applicationUsersDict = userIds.ToDictionary(x => x, x => string.Empty);

        var applicationUsers = await _usersService.GetApplicationUsersAsync(userIds);
        if (applicationUsers != null && applicationUsers.Items != null)
        {
            foreach(var applicationUser in applicationUsers.Items)
            {
                if(!string.IsNullOrWhiteSpace(applicationUser.Id) && !string.IsNullOrWhiteSpace(applicationUser.Email))
                {
                    applicationUsersDict[applicationUser.Id] = applicationUser.Email;
                }
            }
        }

        return Ok(new
        {
            Page = recipeEntitlements.Page,
            TotalPages = recipeEntitlements.TotalPages,
            TotalCount = recipeEntitlements.TotalCount,
            Items = recipeEntitlements.Items?.Select(x => new
            {
                x.Id,
                x.RecipeId,
                x.UserId,
                UserEmail = applicationUsersDict[x.UserId],
                x.Granted,
                x.GrantedBy
            })
        });
    }


    [HttpPost("{recipeId:guid}/share")]
    [Authorize("create:recipe_entitlement")]
    public async Task<ActionResult> CreateRecipeShare([FromRoute] Guid recipeId, [FromBody] ShareRecipeRequest request)
    {
        var userId = _currentUserService.UserId;
        request.RecipeId = recipeId;
        var doesRecipeExist = await DoesRecipeExist(recipeId);
        if (!doesRecipeExist)
        {
            return BadRequest($"Recipe {recipeId} Not Found");
        }
        var getUserRecipePolicy = await GetRecipePolicy(userId, recipeId);
        var canUserShareRecipe = getUserRecipePolicy.Item2.CanShare;
        var isOwner = getUserRecipePolicy.Item2.IsOwner;
        if (!canUserShareRecipe)
        {
            return Forbid();
        }

        if (isOwner && request.Type == "author")
        {
            return Forbid();
        }

        if (!isOwner && (request.Type == "author" || request.Type == "contributor"))
        {
            return Forbid();
        }

        var user = await _usersService.FindApplicationUserByEmailAsync(request.UserEmail);
        if (user == null)
        {
            return BadRequest($"Couldn't Find Active User with Email {request.UserEmail}");
        }
        var dto = new RecipeEntitlementDto
        {
            RecipeId = request.RecipeId,
            UserId = user.Id ?? string.Empty,
            GrantedBy = userId,
            Type = request.Type
        };
        await _usersService.CreateRecipeEntitlementAsync(dto);
        return Ok();
    }

    [HttpPut("{recipeId:guid}/share/{recipeShareId:guid}")]
    [Authorize("update:recipe_entitlement")]
    public async Task<ActionResult> UpdateRecipeShare([FromRoute] Guid recipeId, [FromRoute] Guid recipeShareId, [FromBody] ShareRecipeRequest request)
    {
        var userId = _currentUserService.UserId;
        request.RecipeId = recipeId;
        request.Id = recipeShareId;
        var doesRecipeExist = await DoesRecipeExist(recipeId);
        if (!doesRecipeExist)
        {
            return BadRequest($"Recipe {recipeId} Not Found");
        }
        var getUserRecipePolicy = await GetRecipePolicy(userId, recipeId);
        var isOwner = getUserRecipePolicy.Item2.IsOwner;
        if (!isOwner)
        {
            return Forbid();
        }

        if (isOwner && request.Type == "author")
        {
            return Forbid();
        }

        if (!request.Id.HasValue || request.Id.Value == Guid.Empty)
        {
            return BadRequest($"'Id' attribute is missing from recipe share request");
        }
        var dto = new RecipeEntitlementDto
        {
            Id = request.Id.Value,
            Type = request.Type,
            GrantedBy = userId
        };
        await _usersService.UpdateRecipeEntitlementAsync(dto.Id, dto);
        return Ok();
    }

    [HttpPost("{recipeId:guid}/favorite")]
    [Authorize("create:recipe_favorite")]
    public async Task<ActionResult> FavoriteRecipe([FromRoute] Guid recipeId)
    {
        var userId = _currentUserService.UserId;
        var favorite = new FavoriteDto
        {
            RecipeId = recipeId,
            UserId = userId,
            CreatedBy = userId
        };

        var doesRecipeExist = await DoesRecipeExist(favorite.RecipeId);
        if (!doesRecipeExist)
        {
            return BadRequest($"Recipe {favorite.RecipeId} Not Found");
        }

        await _usersService.CreateFavoriteAsync(favorite);
        return Ok();
    }

    [HttpPost("{recipeId:guid}/un-favorite")]
    [Authorize("delete:recipe_favorite")]
    public async Task<ActionResult> UnFavoriteRecipe([FromRoute] Guid recipeId)
    {
        var userId = _currentUserService.UserId;
        var favorite = new FavoriteDto
        {
            RecipeId = recipeId,
            UserId = userId
        };

        var doesRecipeExist = await DoesRecipeExist(favorite.RecipeId);
        if (!doesRecipeExist)
        {
            return BadRequest($"Recipe {favorite.RecipeId} Not Found");
        }

        await _usersService.DeleteFavoriteAsync(favorite);
        return Ok();
    }
}