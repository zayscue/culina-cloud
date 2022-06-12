using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.Users.Application.ApplicationUsers.Queries.GetApplicationUser;
using CulinaCloud.Users.Application.ApplicationUsers.Commands.UpdateApplicationUser;
using CulinaCloud.Users.Application.ApplicationUsersPolicies.Queries.GetApplicationUsersPolicies;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.Users.Application.ApplicationUsers.Queries.GetApplicationUsers;

namespace CulinaCloud.Users.API.Controllers
{
    [Route("users")]
    public class ApplicationUsersController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<GetApplicationUsersResponse>>> Get([FromQuery] GetApplicationUsersQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetApplicationUserResponse>> Get([FromRoute] string id)
        {
            var response = await Mediator.Send(new GetApplicationUserQuery
            {
                Id = id
            });
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateApplicationUserResponse>> Update([FromRoute] string id, [FromBody] UpdateApplicationUserCommand command)
        {
            command.Id = id;
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("{id}/policies")]
        public async Task<ActionResult<List<GetApplicationUsersPoliciesResponse>>> GetApplicationUsersPolicies([FromRoute] string id, [FromQuery] GetApplicationUsersPoliciesQuery query)
        {
            query.UserId = id;
            var response = await Mediator.Send(query);
            return Ok(response);
        }
    }
}