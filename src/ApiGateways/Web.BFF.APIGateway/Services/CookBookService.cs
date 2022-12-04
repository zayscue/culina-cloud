using System.Text;

namespace CulinaCloud.Web.BFF.APIGateway.Services;

public class CookBookService : ICookBookService
{
    private const string ServiceName = "CookBook";
    private readonly HttpClient _httpClient;

    public CookBookService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<RecipeDto> GetRecipeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/recipes/{id}");
        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            switch(response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new RecipeNotFoundException(id);
                case HttpStatusCode.Unauthorized:
                    throw new InternalServiceAuthorizationException(_httpClient.BaseAddress?.ToString() ?? ServiceName);
                default:
                    throw new InternalServiceException(ServiceName,  response.StatusCode, responseContent);
            }
        }
        var recipe = JsonSerializer.Deserialize<RecipeDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }) ?? new RecipeDto();
        return recipe;
    }

    public async Task<RecipeDto> CreateRecipeAsync(CreateRecipeDto recipe, CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(recipe,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync("/recipes", requestContent, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        if (!response.IsSuccessStatusCode)
        {
            throw new InternalServiceException(ServiceName, response.StatusCode, responseContent);
        }
        var createdRecipe = JsonSerializer.Deserialize<RecipeDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipeDto();
        return createdRecipe;
    }

    public async Task<PaginatedDto<RecipesDto>> GetRecipesAsync(List<Guid> recipeIds, int page, int limit, 
        CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>
        {
            new("limit", limit.ToString()),
            new("page", page.ToString())
        };
        urlParams.AddRange(recipeIds.Select(recipeId => 
            new KeyValuePair<string, string>("recipeIds", recipeId.ToString())));
        using var urlContent = new FormUrlEncodedContent(urlParams);
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/recipes?{query}") ;
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var recipeResults = JsonSerializer.Deserialize<PaginatedDto<RecipesDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new PaginatedDto<RecipesDto>();
        return recipeResults;
    }

    public async Task<PaginatedDto<RecipesDto>> GetRecipesAsync(string name, int page, int limit, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>
        {
            new("limit", limit.ToString()),
            new("page", page.ToString())
        };
        if (!string.IsNullOrWhiteSpace(name))
        {
            urlParams.Add(new KeyValuePair<string, string>("name", name));
        }
        using var urlContent = new FormUrlEncodedContent(urlParams);
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/recipes?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var recipeResults = JsonSerializer.Deserialize<PaginatedDto<RecipesDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new PaginatedDto<RecipesDto>();
        return recipeResults;
    }

    public async Task UpdateRecipeAsync(Guid recipeId, RecipeDto recipe,
        CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(recipe,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PutAsync($"/recipes/{recipeId}", requestContent, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var updatedRecipe = JsonSerializer.Deserialize<RecipeDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipeDto();
    }

    public async Task<RecipeNutritionDto> CreateRecipeNutritionAsync(Guid recipeId, RecipeNutritionDto nutrition, CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(nutrition,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync($"/recipes/{recipeId}/nutrition", 
            requestContent, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var createdRecipeNutrition = JsonSerializer.Deserialize<RecipeNutritionDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipeNutritionDto();
        return createdRecipeNutrition;
    }

    public async Task<RecipeNutritionDto> GetRecipeNutritionAsync(Guid recipeId, CancellationToken cancellation = default)
    {
        using var response = await _httpClient.GetAsync($"/recipes/{recipeId}/nutrition", cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var nutrition = JsonSerializer.Deserialize<RecipeNutritionDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipeNutritionDto();
        return nutrition;
    }

    public async Task UpdateRecipeNutritionAsync(Guid recipeId, RecipeNutritionDto recipeNutrition,
        CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(recipeNutrition,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PutAsync($"/recipes/{recipeId}/nutrition", requestContent, 
            cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var updatedRecipeNutrition = JsonSerializer.Deserialize<RecipeNutritionDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipeNutritionDto();
    }

    public async Task<List<RecipeStepDto>> GetRecipeStepsAsync(Guid recipeId, CancellationToken cancellation = default)
    {
        using var response = await _httpClient.GetAsync($"/recipes/{recipeId}/steps", cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var steps = JsonSerializer.Deserialize<List<RecipeStepDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new List<RecipeStepDto>();
        return steps;
    }

    public async Task<RecipeStepDto> GetRecipeStepAsync(Guid recipeId, int order, CancellationToken cancellation = default)
    {
        using var response = await _httpClient.GetAsync($"/recipes/{recipeId}/steps/{order}", cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var step = JsonSerializer.Deserialize<RecipeStepDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipeStepDto();
        return step;
    }

    public async Task BatchUpdateRecipeStepsAsync(Guid recipeId, List<RecipeStepDto> steps,
        CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(steps,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PutAsync($"/recipes/{recipeId}/steps", requestContent, 
            cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var updatedRecipeNutrition = JsonSerializer.Deserialize<List<RecipeStepDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new List<RecipeStepDto>();
    }

    public async Task<RecipeImageDto> CreateRecipeImageAsync(Guid recipeId, RecipeImageDto image, CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(image,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync($"/recipes/{recipeId}/images", requestContent, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var createdRecipeImage = JsonSerializer.Deserialize<RecipeImageDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipeImageDto();
        return createdRecipeImage;
    }

    public async Task<List<RecipeImageDto>> GetRecipeImagesAsync(Guid recipeId, CancellationToken cancellation = default)
    {
        using var response = await _httpClient.GetAsync($"/recipes/{recipeId}/images", cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var images = JsonSerializer.Deserialize<List<RecipeImageDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new List<RecipeImageDto>();
        return images;
    }

    public async Task BatchUpdateRecipeImagesAsync(Guid recipeId, List<RecipeImageDto> images, CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(images,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PutAsync($"/recipes/{recipeId}/images", requestContent, 
            cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var updatedRecipeNutrition = JsonSerializer.Deserialize<List<RecipeStepDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new List<RecipeStepDto>();
    }

    public async Task<RecipeImageDto> GetRecipeImageAsync(Guid recipeId, Guid imageId, CancellationToken cancellation = default)
    {
        using var response = await _httpClient.GetAsync($"/recipes/{recipeId}/images/{imageId}", cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var image = JsonSerializer.Deserialize<RecipeImageDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipeImageDto();
        return image;
    }

    public async Task<List<RecipeIngredientDto>> GetRecipeIngredientsAsync(Guid recipeId, CancellationToken cancellation = default)
    {
        using var response = await _httpClient.GetAsync($"/recipes/{recipeId}/ingredients", cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var ingredients = JsonSerializer.Deserialize<List<RecipeIngredientDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new List<RecipeIngredientDto>();
        return ingredients;
    }

    public async Task<RecipeIngredientDto> CreateRecipeIngredientAsync(Guid recipeId, RecipeIngredientDto recipeIngredient,
        CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(recipeIngredient,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        using var response =
            await _httpClient.PostAsync($"/recipes/{recipeId}/ingredients", requestContent, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var createdRecipeIngredient = JsonSerializer.Deserialize<RecipeIngredientDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipeIngredientDto();
        return createdRecipeIngredient;
    }

    public async Task BatchUpdateRecipeIngredientsAsync(Guid recipeId, List<RecipeIngredientDto> ingredients, CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(ingredients,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PutAsync($"/recipes/{recipeId}/ingredients",
            requestContent, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var updatedRecipeIngredients = JsonSerializer.Deserialize<List<RecipeIngredientDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new List<RecipeIngredientDto>();
    }

    public async Task<RecipeIngredientDto> GetRecipeIngredientAsync(Guid recipeId, Guid recipeIngredientId, 
        CancellationToken cancellation = default)
    {
        using var response =
            await _httpClient.GetAsync($"/recipes/{recipeId}/ingredients/{recipeIngredientId}", cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var recipeIngredient = JsonSerializer.Deserialize<RecipeIngredientDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipeIngredientDto();
        return recipeIngredient;
    }

    public async Task<List<RecipeTagDto>> GetRecipeTagsAsync(Guid recipeId, CancellationToken cancellation = default)
    {
        using var response = await _httpClient.GetAsync($"/recipes/{recipeId}/tags", cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var tags = JsonSerializer.Deserialize<List<RecipeTagDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new List<RecipeTagDto>();
        return tags;
    }

    public async Task<RecipeTagDto> CreateRecipeTagAsync(Guid recipeId, RecipeTagDto tag,
        CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(tag,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync($"/recipes/{recipeId}/tags", requestContent, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var createdRecipeTag = JsonSerializer.Deserialize<RecipeTagDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipeTagDto();
        return createdRecipeTag;
    }

    public async Task BatchUpdateRecipeTagsAsync(Guid recipeId, List<RecipeTagDto> tags, CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(tags,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PutAsync($"/recipes/{recipeId}/tags",
            requestContent, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var updatedRecipeTags = JsonSerializer.Deserialize<List<RecipeTagDto>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new List<RecipeTagDto>();
    }

    public async Task<RecipeTagDto> GetRecipeTagAsync(Guid recipeId, Guid tagId, CancellationToken cancellation = default)
    {
        using var response = await _httpClient.GetAsync($"/recipes/{recipeId}/tags/{tagId}", cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var recipeTag = JsonSerializer.Deserialize<RecipeTagDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipeTagDto();
        return recipeTag;
    }

    public async Task<RecipeStatisticsDto> GetRecipeStatisticsAsync(CancellationToken cancellation = default)
    {
        using var response = await _httpClient.GetAsync($"/statistics", cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        var recipeStatistics = JsonSerializer.Deserialize<RecipeStatisticsDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new RecipeStatisticsDto();
        return recipeStatistics;
    }

    public async Task<PaginatedDto<IngredientDto>> GetIngredientsAsync(string name, int page, int limit, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>
        {
            new("limit", limit.ToString()),
            new("page", page.ToString())
        };
        if (!string.IsNullOrWhiteSpace(name))
        {
            urlParams.Add(new KeyValuePair<string, string>("name", name));
        }
        using var urlContent = new FormUrlEncodedContent(urlParams);
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/ingredients?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        if (!response.IsSuccessStatusCode)
        {
            throw new InternalServiceException(ServiceName, response.StatusCode, responseContent);
        }
        var ingredients = JsonSerializer.Deserialize<PaginatedDto<IngredientDto>>(
            responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new PaginatedDto<IngredientDto>();
        return ingredients;
    }

    public async Task<PaginatedDto<TagDto>> GetTagsAsync(string name, int page, int limit, CancellationToken cancellation = default)
    {
        var urlParams = new List<KeyValuePair<string, string>>
        {
            new("limit", limit.ToString()),
            new("page", page.ToString())
        };
        if (!string.IsNullOrWhiteSpace(name))
        {
            urlParams.Add(new KeyValuePair<string, string>("name", name));
        }
        using var urlContent = new FormUrlEncodedContent(urlParams);
        var query = await urlContent.ReadAsStringAsync(cancellation);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/tags?{query}");
        using var response = await _httpClient.SendAsync(request, cancellation);
        var responseContent = await response.Content.ReadAsStringAsync(cancellation);
        if (!response.IsSuccessStatusCode)
        {
            throw new InternalServiceException(ServiceName, response.StatusCode, responseContent);
        }
        var tags = JsonSerializer.Deserialize<PaginatedDto<TagDto>>(
            responseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new PaginatedDto<TagDto>();
        return tags;
    }
}
