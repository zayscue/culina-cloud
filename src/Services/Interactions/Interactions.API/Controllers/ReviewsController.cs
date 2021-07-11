using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.Interactions.Application.Reviews.Commands.CreateReview;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.Interactions.API.Controllers
{
    [Route("interactions/[controller]")]
    public class ReviewsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateReviewResponse>> Create(CreateReviewCommand command)
        {
            var vm = await Mediator.Send(command);
            return vm;
        }
    }
}
