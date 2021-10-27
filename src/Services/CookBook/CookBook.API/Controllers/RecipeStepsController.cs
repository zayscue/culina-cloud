using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.CookBook.Application.RecipeSteps.Queries.GetRecipeStep;
using CulinaCloud.CookBook.Application.RecipeSteps.Queries.GetRecipeSteps;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.CookBook.API.Controllers
{
    [Route("recipes")]
    public class RecipeStepsController : ApiControllerBase
    {
        [HttpGet("{id:guid}/steps")]
        public async Task<ActionResult<List<GetRecipeStepsResponse>>> Get(Guid id)
        {
            var response = await Mediator.Send(new GetRecipeStepsQuery { RecipeId = id });
            return Ok(response);
        }

        [HttpGet("{id:guid}/steps/{order:int}")]
        public async Task<ActionResult<GetRecipeStepResponse>> Get(Guid id, int order)
        {
            var response = await Mediator.Send(new GetRecipeStepQuery { RecipeId = id, Order = order });
            return Ok(response);
        }
    }
}