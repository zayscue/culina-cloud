using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.Common;
using CulinaCloud.CookBook.Application.Common.Interfaces;

namespace CulinaCloud.CookBook.Infrastructure.EventStore
{
  public class EventStoreService : IEventStoreService
    {
        private readonly ITokenService _tokenService;
        private readonly HttpClient _httpClient;

        public EventStoreService(ITokenService tokenService, HttpClient httpClient)
        {
            _tokenService = tokenService;
            _httpClient = httpClient;
        }

        public async Task StoreEventsAsync(Guid aggregateId, IEnumerable<AggregateEvent> events, CancellationToken cancellationToken = default)
        {
            var (tokenType, accessToken) = await _tokenService.GetToken(cancellationToken);
            var json = JsonSerializer.Serialize(events.Select(x => new
            {
                x.EventId,
                x.EventName,
                x.AggregateType,
                x.Data,
                x.Occurred,
                x.Details,
                x.RaisedBy
            }));
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_httpClient.BaseAddress, $"/eventstore/store/{aggregateId}"),
                Headers =
                {
                    { HttpRequestHeader.Authorization.ToString(), $"{tokenType} {accessToken}" }
                },
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            await _httpClient.SendAsync(request, cancellationToken);
        }

        public async Task<IEnumerable<AggregateEvent>> LoadEventsAsync(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            var (tokenType, accessToken) = await _tokenService.GetToken(cancellationToken);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_httpClient.BaseAddress, $"/eventstore/load/{aggregateId}"),
                Headers =
                {
                    { HttpRequestHeader.Authorization.ToString(), $"{tokenType} {accessToken}" }
                }
            };
            var response = await _httpClient.SendAsync(request, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var aggregateEvents = JsonSerializer.Deserialize<List<AggregateEvent>>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return aggregateEvents;
        }

        public async Task<bool> CheckHealth(CancellationToken cancellationToken = default)
        {
            var request = new  HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_httpClient.BaseAddress, $"/health")
            };
            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode) return false;
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!string.Equals(responseContent, "Healthy", StringComparison.OrdinalIgnoreCase)) return false;
            return true;
        }
  }
}