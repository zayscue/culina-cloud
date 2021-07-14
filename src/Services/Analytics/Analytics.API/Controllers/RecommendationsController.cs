using CulinaCloud.BuildingBlocks.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.Analytics.API.Controllers
{
    [Route("analytics/[controller]")]
    public class RecommendationsController : ApiControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Hello World!");
        }
    }
}
