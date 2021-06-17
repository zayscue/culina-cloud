using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Culina.CookBook.Application.Recipes.Commands.CreateRecipe;
using Culina.CookBook.Application.Recipes.Queries.GetRecipe;

namespace Culina.CookBook.API.Controllers
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
