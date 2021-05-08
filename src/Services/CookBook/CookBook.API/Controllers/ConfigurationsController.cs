using System.Threading.Tasks;
using Culina.CookBook.Infrastructure.EventStore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Culina.CookBook.API.Controllers
{
    public class ConfigurationsController : ApiControllerBase
    {
        private readonly EventStoreSecretsProvider _secretsProvider;

        public ConfigurationsController(EventStoreSecretsProvider secretsProvider) : base()
        {
            _secretsProvider = secretsProvider;
        }
        
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var (clientId, clientSecret) = await _secretsProvider.GetSecrets();
            var response = new
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };
            return Ok(response);
        }
    }
}