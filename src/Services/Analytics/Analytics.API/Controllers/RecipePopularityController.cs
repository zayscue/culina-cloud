using System;
using System.Threading.Tasks;
using CulinaCloud.Analytics.Application.RecipePopularities.Commands.CreateRecipePopularity;
using CulinaCloud.Analytics.Application.RecipePopularities.Commands.UpdateRecipePopularity;
using CulinaCloud.Analytics.Application.RecipePopularities.Queries.GetRecipePopularities;
using CulinaCloud.Analytics.Application.RecipePopularities.Queries.GetRecipePopularity;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.Analytics.API.Controllers
{
    [Authorize]
    [Route("analytics/recipe-popularity")]
    public class RecipePopularityController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<GetRecipePopularitiesResponse>>> Get([FromQuery] GetRecipePopularitiesQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{recipeId:guid}")]
        public async Task<ActionResult<GetRecipePopularityQuery>> Get(Guid recipeId)
        {
            var response = await Mediator.Send(new GetRecipePopularityQuery
            {
                RecipeId = recipeId
            });
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CreateRecipePopularityResponse>> Create([FromBody] CreateRecipePopularityCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<UpdateRecipePopularityResponse>> Update([FromBody] UpdateRecipePopularityCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}
