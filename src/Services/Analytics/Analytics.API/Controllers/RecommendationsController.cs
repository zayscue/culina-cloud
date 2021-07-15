using System;
using System.Threading.Tasks;
using CulinaCloud.Analytics.Application.Recommendations.Queries.GetPersonalRecipeRecommendations;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.Analytics.API.Controllers
{
    [Route("analytics/recommendations")]
    public class RecommendationsController : ApiControllerBase
    {
        [HttpGet("personal-recipe-recommendations")]
        public async Task<ActionResult<PaginatedList<Guid>>> GetPersonalRecipeRecommendations(
            [FromQuery] GetPersonalRecipeRecommendationsQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("similar-recipes")]
        public ActionResult GetSimilarRecipes()
        {
            return Ok("Work in-progress!");
        }
    }
}