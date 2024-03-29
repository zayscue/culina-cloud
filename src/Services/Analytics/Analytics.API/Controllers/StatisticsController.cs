﻿using CulinaCloud.BuildingBlocks.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CulinaCloud.Analytics.Application.Statistics.Queries.GetStatistics;
using Microsoft.AspNetCore.Authorization;

namespace CulinaCloud.Analytics.API.Controllers
{
    [Authorize]
    [Route("analytics/statistics")]
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
