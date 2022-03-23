using System.Threading.Tasks;
using CulinaCloud.Analytics.Application.RecipePopularities.Commands.CreateRecipePopularity;
using CulinaCloud.Analytics.Application.RecipePopularities.Queries.GetRecipePopularities;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.Analytics.API.Controllers
{
    [Route("analytics/recipe-popularity")]
    public class RecipePopularityController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<GetRecipePopularitiesResponse>>> Get([FromQuery] int? limit,
            [FromQuery] string orderBy = "", [FromQuery] bool descending = false, [FromQuery] int page = 1)
        {
            var response = await Mediator.Send(new GetRecipePopularitiesQuery
            {
                Limit = limit,
                Page = page,
                OrderBy = orderBy,
                Descending = descending
            });
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Policy = "CreateRecipePopularity")]
        public async Task<ActionResult<CreateRecipePopularityResponse>> Create([FromBody] CreateRecipePopularityCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}
