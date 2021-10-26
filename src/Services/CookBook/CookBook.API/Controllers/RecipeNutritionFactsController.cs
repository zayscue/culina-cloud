using System;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.CookBook.Application.RecipeNutritionFacts.Commands.CreateRecipeNutrition;
using CulinaCloud.CookBook.Application.RecipeNutritionFacts.Commands.UpdateRecipeNutrition;
using CulinaCloud.CookBook.Application.RecipeNutritionFacts.Queries.GetRecipeNutrition;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.CookBook.API.Controllers
{
    [Route("recipes")]
    public class RecipeNutritionFactsController : ApiControllerBase
    {
        [HttpGet("{id:guid}/nutrition")]
        public async Task<ActionResult<GetRecipeNutritionResponse>> Get(Guid id)
        {
            var response = await Mediator.Send(new GetRecipeNutritionQuery { RecipeId = id });
            return Ok(response);
        }

        [HttpPost("{id:guid}/nutrition")]
        public async Task<ActionResult<CreateRecipeNutritionResponse>> Create(Guid id, CreateRecipeNutritionCommand command)
        {
            command.RecipeId = id;
            var response = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new {id = response.RecipeId}, response);
        }

        [HttpPut("{id:guid}/nutrition")]
        public async Task<ActionResult<UpdateRecipeNutritionResponse>> Update(Guid id, UpdateRecipeNutritionCommand command)
        {
            command.RecipeId = id;
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}