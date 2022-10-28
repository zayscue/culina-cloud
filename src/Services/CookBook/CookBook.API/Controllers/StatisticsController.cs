using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.CookBook.Application.CookBookStatistics.Queries.GetCookBookStatistics;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CulinaCloud.CookBook.API.Controllers
{
    [Route("statistics")]
    public class StatisticsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetCookBookStatisticsResponse>> Get()
        {
            var response = await Mediator.Send(new GetCookBookStatisticsQuery());
            return Ok(response);
        }
    }
}
