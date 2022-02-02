using System;
using System.Threading.Tasks;
using CulinaCloud.Analytics.Application.RecipeSimilarities.Queries.GetRecipeSimilarities;
using CulinaCloud.Analytics.Application.Recommendations.Queries.GetPersonalRecipeRecommendations;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.Analytics.API.Controllers
{
    [Route("analytics/recommendations")]
    public class RecommendationsController : ApiControllerBase
    {
        [HttpGet("personal-recipe-recommendations")]
        //[Authorize(Policy = "ReadPersonalRecipeRecommendations")]
        public async Task<ActionResult<PaginatedList<PersonalRecipeRecommendationsResponse>>> GetPersonalRecipeRecommendations(
            [FromQuery] GetPersonalRecipeRecommendationsQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("similar-recipes")]
        //[Authorize(Policy = "ReadSimilarRecipes")]
        public async Task<ActionResult<PaginatedList<GetRecipeSimilaritiesResponse>>> GetSimilarRecipes([FromQuery] GetRecipeSimilaritiesQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }
    }
}