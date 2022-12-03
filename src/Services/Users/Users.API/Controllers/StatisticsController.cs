using CulinaCloud.BuildingBlocks.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CulinaCloud.Users.Application.Statistics.Queries.GetStatistics;

namespace CulinaCloud.Users.API.Controllers
{
    [Route("statistics")]
    public class StatisticsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetStatisticsResponse>> Get()
        {
            var response = await Mediator.Send(new GetStatisticsQuery());
            return Ok(response);
        }
    }
}
