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
    [Authorize]
    [Route("interactions/[controller]")]
    public class ReviewsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateReviewResponse>> Create(CreateReviewCommand command)
        {
            var scopes = User.FindFirstValue("scope");
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var scopesExists = !string.IsNullOrWhiteSpace(scopes);
            var containsCreateReviewsScope = scopesExists && scopes.Contains("create:reviews");
            var userIdExists = !string.IsNullOrWhiteSpace(command.UserId);
            var userIdMatches = !string.IsNullOrWhiteSpace(currentUser)
                                && currentUser.Equals(command.UserId, StringComparison.Ordinal);
            switch (scopesExists)
            {
                case false:
                    return Unauthorized();
                case true when userIdExists && !containsCreateReviewsScope && !userIdMatches:
                    return Unauthorized();
                default:
                {
                    var vm = await Mediator.Send(command);
                    return vm;
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<GetReviewsResponse>>> Get([FromQuery] GetReviewsQuery query)
        {
            var scopes = User.FindFirstValue("scope");
            var scopesExists = !string.IsNullOrWhiteSpace(scopes);
            var containsReadReviewsScope = scopesExists && scopes.Contains("read:reviews");
            switch (scopesExists)
            {
                case false:
                    return Unauthorized();
                case true when !containsReadReviewsScope:
                    return Unauthorized();
                default:
                {
                    var vm = await Mediator.Send(query);
                    return vm;
                }
            }
        }
    }
}
