using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Culina.CookBook.API.Controllers
{
    public class ConfigurationsController : ApiControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigurationsController(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }
        
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var clientId = _configuration["ClientId"];
            var clientSecret = _configuration["ClientSecret"];
            var response = new
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };
            return Ok(response);
        }
    }
}