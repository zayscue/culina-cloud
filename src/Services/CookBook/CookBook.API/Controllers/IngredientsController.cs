using System;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.CookBook.Application.Ingredients.Commands.CreateIngredient;
using CulinaCloud.CookBook.Application.Ingredients.Queries.GetIngredient;
using CulinaCloud.CookBook.Application.Ingredients.Queries.GetIngredients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CulinaCloud.CookBook.API.Controllers
{
    [Authorize]
    [Route("ingredients")]
    public class IngredientsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<GetIngredientsResponse>>> Get([FromQuery] GetIngredientsQuery query)
        {
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetIngredientResponse>> Get(Guid id)
        {
            var vm = await Mediator.Send(new GetIngredientQuery {Id = id});
            return Ok(vm);
        }

        [HttpPost]
        public async Task<ActionResult<CreateIngredientResponse>> Create(CreateIngredientCommand command)
        {
            var vm = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new {id = vm.Id}, vm);
        }
    }
}
