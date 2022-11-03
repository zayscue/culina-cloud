using CulinaCloud.Analytics.Application.RecipeStatistics.Queries.GetRecipeStatistics;
using CulinaCloud.BuildingBlocks.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CulinaCloud.Analytics.API.Controllers
{
    [Route("analytics/statistics")]
    public class StatisticsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetRecipeStatisticsResponse>> Get()
        {
            var response = await Mediator.Send(new GetRecipeStatisticsQuery());
            return Ok(response);
        }
    }
}
