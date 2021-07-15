using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.Analytics.API.Controllers
{
    [Route("analytics/recipe-popularity")]
    public class RecipePopularityController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Create()
        {
            return Ok("Work in-progress!");
        }
    }
}
