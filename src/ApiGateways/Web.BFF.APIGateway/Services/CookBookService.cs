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
                    throw new InternalServiceException(responseContent);
            }
        }
        var recipe = JsonSerializer.Deserialize<RecipeDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }) ?? new RecipeDto();
        return recipe;
    }

    public async Task<RecipeDto> CreateRecipeAsync(RecipeDto recipe, CancellationToken cancellation = default)
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
}
