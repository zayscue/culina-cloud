using System.Threading.Tasks;
using CulinaCloud.Analytics.Application.RecipeSimilarities.Commands.CreateRecipeSimilarity;
using CulinaCloud.BuildingBlocks.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.Analytics.API.Controllers
{
    [Route("analytics/recipe-similarity")]
    public class RecipeSimilarityController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateRecipeSimilarityResponse>> Create([FromBody] CreateRecipeSimilarityCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}
