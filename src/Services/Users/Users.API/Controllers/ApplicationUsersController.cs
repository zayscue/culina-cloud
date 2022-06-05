using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.Users.Application.ApplicationUsers.Queries.GetApplicationUser;
using CulinaCloud.Users.Application.ApplicationUsers.Commands.CreateApplicationUser;
using CulinaCloud.Users.Application.ApplicationUsers.Commands.UpdateApplicationUser;
using CulinaCloud.Users.Application.ApplicationUsersPolicies.Queries.GetApplicationUsersPolicies;
using System.Collections.Generic;

namespace CulinaCloud.Users.API.Controllers
{
    [Route("users")]
    public class ApplicationUsersController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateApplicationUserResponse>> Get([FromBody] CreateApplicationUserCommand command)
        {
            var response = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new {id = response.Id}, response);
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