using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.CookBook.Application.RecipeIngredients.Queries.GetRecipeIngredients;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.CookBook.API.Controllers
{
    [Route("recipes")]
    public class RecipeIngredientsController : ApiControllerBase
    {
        [HttpGet("{id:guid}/ingredients")]
        public async Task<ActionResult<List<GetRecipeIngredientsResponse>>> Get(Guid id)
        {
            var response = await Mediator.Send(new GetRecipeIngredientsQuery { RecipeId = id });
            return Ok(response);
        }
    }
}