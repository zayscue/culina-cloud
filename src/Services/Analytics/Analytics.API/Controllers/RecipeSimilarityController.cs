using System.Threading.Tasks;
using CulinaCloud.Analytics.Application.RecipeSimilarities.Commands.CreateRecipeSimilarity;
using CulinaCloud.BuildingBlocks.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.Analytics.API.Controllers
{
    [Authorize]
    [Route("analytics/recipe-similarity")]
    public class RecipeSimilarityController : ApiControllerBase
    {
        [HttpPost]
        [Authorize(Policy = "CreateRecipeSimilarity")]
        public async Task<ActionResult<CreateRecipeSimilarityResponse>> Create([FromBody] CreateRecipeSimilarityCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}
