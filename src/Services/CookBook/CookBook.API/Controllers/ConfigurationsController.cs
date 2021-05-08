using System;
using System.Threading.Tasks;
using Culina.CookBook.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Culina.CookBook.API.Controllers
{
    public class ConfigurationsController : ApiControllerBase
    {
        private readonly IEventStoreService _eventStoreService;

        public ConfigurationsController(IEventStoreService eventStoreService) : base()
        {
            _eventStoreService = eventStoreService;
        }
        
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var aggregateId = new Guid("b84af811-89ff-4953-834c-f37ed1db486d");
            var response = await _eventStoreService.LoadEventsAsync(aggregateId);
            return Ok(response);
        }
    }
}