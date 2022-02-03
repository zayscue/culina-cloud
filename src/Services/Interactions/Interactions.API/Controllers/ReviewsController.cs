using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.Interactions.Application.Reviews.Commands.CreateReview;
using CulinaCloud.Interactions.Application.Reviews.Queries.GetReviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.Interactions.API.Controllers
{
    [Route("interactions/reviews")]
    public class ReviewsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateReviewResponse>> Create(CreateReviewCommand command)
        {
            var vm = await Mediator.Send(command);
            return vm;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<GetReviewsResponse>>> Get([FromQuery] GetReviewsQuery query)
        {
            var vm = await Mediator.Send(query);
            return vm;
        }
    }
}
