﻿using CulinaCloud.Analytics.Application.RecentRecipes.Queries.GetRecentRecipes;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CulinaCloud.Analytics.API.Controllers
{
    [Authorize]
    [Route("analytics/recent-recipes")]
    public class RecentRecipesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<RecentRecipeResponse>>> Get([FromQuery] GetRecentRecipesQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }
    }
}
