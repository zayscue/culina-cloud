﻿using CulinaCloud.BuildingBlocks.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CulinaCloud.Users.Application.Statistics.Queries.GetStatistics;
using Microsoft.AspNetCore.Authorization;

namespace CulinaCloud.Users.API.Controllers
{
    [Authorize]
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
