using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.Authentication.Abstractions;
using CulinaCloud.Interactions.Application.Interfaces;

namespace CulinaCloud.Interactions.Infrastructure.CookBook
{
    public class RecipesService : IRecipesService
    {
        private readonly HttpClient _httpClient;

        public RecipesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CheckHealth(CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(_httpClient.BaseAddress, $"/health")
                };
                var response = await _httpClient.SendAsync(request, cancellationToken);
                if (!response.IsSuccessStatusCode) return false;
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                return string.Equals(responseContent, "Healthy", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RecipeExistsAsync(Guid recipeId, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_httpClient.BaseAddress, $"/recipes/{recipeId}")
            };
            var response = await _httpClient.SendAsync(request, cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}
