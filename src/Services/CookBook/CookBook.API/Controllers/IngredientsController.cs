using System;
using System.Threading.Tasks;
using CulinaCloud.CookBook.Application.Common.Models;
using CulinaCloud.CookBook.Application.Ingredients.Commands.CreateIngredient;
using CulinaCloud.CookBook.Application.Ingredients.Queries.GetIngredient;
using CulinaCloud.CookBook.Application.Ingredients.Queries.GetIngredients;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.CookBook.API.Controllers
{
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
