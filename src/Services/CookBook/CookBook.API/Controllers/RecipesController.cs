using System;
using System.Threading.Tasks;
using CulinaCloud.CookBook.Application.Recipes.Commands.CreateRecipe;
using CulinaCloud.CookBook.Application.Recipes.Queries.GetRecipe;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.CookBook.API.Controllers
{
    public class RecipesController : ApiControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetRecipeResponse>> Get(Guid id)
        {
            var vm = await Mediator.Send(new GetRecipeQuery {Id = id});
            return Ok(vm);
        }

        [HttpPost]
        public async Task<ActionResult<CreateRecipeResponse>> Create(CreateRecipeCommand command)
        {
            var vm = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new {id = vm.Id}, vm);
        }
    }
}
