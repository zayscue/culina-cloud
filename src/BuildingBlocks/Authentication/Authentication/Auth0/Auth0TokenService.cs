using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using CulinaCloud.BuildingBlocks.Common.Interfaces;
using CulinaCloud.BuildingBlocks.Authentication.Auth0.Settings;
using CulinaCloud.BuildingBlocks.Authentication.Auth0.Secrets.Providers;

namespace CulinaCloud.BuildingBlocks.Authentication.Auth0
{
    public class Auth0TokenService : ITokenService
    {
        private readonly IDateTime _dateTime;
        private readonly IOptions<Auth0Settings> _settings;
        private readonly Auth0SecretsProvider _secretsProvider;
        private readonly string _audience;

        private Auth0Token token;

        public Auth0TokenService(
            IDateTime dateTime,
            IOptions<Auth0Settings> settings,
            Auth0SecretsProvider secretsProvider,
            string audience)
        {
            _dateTime = dateTime;
            _settings = settings;
            _secretsProvider = secretsProvider;
            _audience = audience;
            token = new Auth0Token(_dateTime);
        }


        public async Task<(string TokenType, string AccessToken)> GetToken(CancellationToken cancellationToken = default)
        {
            if (token.IsValidAndNotExpiring) return (token.TokenType, token.AccessToken);
            token = await GetNewAccessToken(cancellationToken);
            return (token.TokenType, token.AccessToken);
        }

        private async Task<Auth0Token> GetNewAccessToken(CancellationToken cancellationToken)
        {
            var secrets = await _secretsProvider.GetSecrets(cancellationToken);
            var auth0TokenUrl = _settings.Value.TokenUrl;

            using var auth0HttpClient = new HttpClient();
            auth0HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonSerializer.Serialize(new
            {
                client_id = secrets.ClientId,
                client_secret = secrets.ClientSecret,
                audience = _audience,
                grant_type = "client_credentials"
            });
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await auth0HttpClient.PostAsync(auth0TokenUrl, requestContent, cancellationToken);
            if (!response.IsSuccessStatusCode) throw new Exception("Unable to retrieve access token from Auth0");
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var auth0TokenResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
            var tokenType = auth0TokenResponse["token_type"].ToString();
            var accessToken = auth0TokenResponse["access_token"].ToString();
            var expiresIn = int.Parse(auth0TokenResponse["expires_in"].ToString());
            return new Auth0Token(_dateTime)
            {
                AccessToken = accessToken,
                TokenType = tokenType,
                ExpiresIn = expiresIn,
                ExpiresAt = _dateTime.Now.AddSeconds(expiresIn)
            };
        }
    }
}
