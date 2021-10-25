using System;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.CookBook.Application.RecipeNutritionFacts.Commands.CreateRecipeNutrition;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.CookBook.API.Controllers
{
    [Route("recipes")]
    public class RecipeNutritionFactsController : ApiControllerBase
    {
        [HttpPost("{id:guid}/nutrition")]
        public async Task<ActionResult<CreateRecipeNutritionResponse>> CreateRecipeNutrition(Guid id, CreateRecipeNutritionCommand command)
        {
            command.RecipeId = id;
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}