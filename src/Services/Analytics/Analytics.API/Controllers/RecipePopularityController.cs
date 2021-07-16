using System.Threading.Tasks;
using CulinaCloud.Analytics.Application.RecipePopularities.Commands.CreateRecipePopularity;
using CulinaCloud.BuildingBlocks.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.Analytics.API.Controllers
{
    [Route("analytics/recipe-popularity")]
    public class RecipePopularityController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateRecipePopularityResponse>> Create([FromBody] CreateRecipePopularityCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}
