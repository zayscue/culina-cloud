using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Application.Common.Models;
using CulinaCloud.BuildingBlocks.Common;
using Microsoft.Extensions.Configuration;

namespace Culina.CookBook.Infrastructure.EventStore
{
    public class EventStoreService : IEventStoreService
    {
        private readonly IConfiguration _configuration;
        private readonly EventStoreSecretsProvider _secretsProvider;

        public EventStoreService(IConfiguration configuration, EventStoreSecretsProvider secretsProvider)
        {
            _configuration = configuration;
            _secretsProvider = secretsProvider;
        }
        
        public Task StoreEventsAsync(Guid aggregateId, IEnumerable<AggregateEvent> events, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GenericAggregateEvent>> LoadEventsAsync(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            var (clientId, clientSecret) = await _secretsProvider.GetSecrets(cancellationToken);
            var eventStoreAudience = _configuration["EventStore:Audience"];
            var eventStoreUrl = _configuration["EventStore:Url"];
            var auth0Domain = _configuration["Auth0:Domain"];
            
            using var auth0HttpClient = new HttpClient() { BaseAddress = new Uri(auth0Domain) };
            auth0HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonSerializer.Serialize(new
            {
                client_id = clientId,
                client_secret = clientSecret,
                audience = eventStoreAudience,
                grant_type = "client_credentials"
            });
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await auth0HttpClient.PostAsync("/oauth/token", requestContent, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var auth0TokenResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
            var tokenType = auth0TokenResponse["token_type"].ToString();
            var accessToken = auth0TokenResponse["access_token"].ToString();

            using var eventStoreHttpClient = new HttpClient() {BaseAddress = new Uri(eventStoreUrl)};
            eventStoreHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            eventStoreHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(tokenType, accessToken);
            var loadResponse =
                await eventStoreHttpClient.GetAsync($"/eventstore/load/{aggregateId}", cancellationToken);
            var loadResponseContent = await loadResponse.Content.ReadAsStringAsync(cancellationToken);
            var aggregateEvents = JsonSerializer.Deserialize<List<GenericAggregateEvent>>(loadResponseContent, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return aggregateEvents;
        }
    }
}