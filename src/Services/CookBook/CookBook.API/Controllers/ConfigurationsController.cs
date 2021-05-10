using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Domain.Entities;
using Culina.CookBook.Domain.Events;
using CulinaCloud.BuildingBlocks.Common;
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
        
        [HttpGet("load/{aggregateId:guid}")]
        public async Task<ActionResult> Load(Guid aggregateId)
        {
            var aggregateEvents = await _eventStoreService.LoadEventsAsync(aggregateId);
            return Ok(aggregateEvents);
        }
        
        [HttpGet("store/{aggregateId:guid}")]
        public async Task<ActionResult> Store(Guid aggregateId)
        {
            var ingredient = new Ingredient
            {
                IngredientName = "Chicken",
                Id = aggregateId
            };
            var ingredientCreatedEvent = new IngredientCreatedEvent(ingredient);
            var events = new List<AggregateEvent> {ingredientCreatedEvent};
            await _eventStoreService.StoreEventsAsync(aggregateId, events);
            return Ok(aggregateId);
        }
    }
}