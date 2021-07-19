using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.Authentication.Abstractions;
using CulinaCloud.Users.Application.Interfaces;

namespace CulinaCloud.Users.Infrastructure.CookBook
{
    public class RecipesService : IRecipesService
    {
        private readonly ITokenService _tokenService;
        private readonly HttpClient _httpClient;

        public RecipesService(ITokenService tokenService, HttpClient httpClient)
        {
            _tokenService = tokenService;
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
            var (tokenType, accessToken) = await _tokenService.GetToken(cancellationToken);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_httpClient.BaseAddress, $"/cookbook/recipes/{recipeId}"),
                Headers =
                {
                    { HttpRequestHeader.Authorization.ToString(), $"{tokenType} {accessToken}" }
                }
            };
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
