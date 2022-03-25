using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.Users.Application.RecipeEntitlements.Commands.CreateRecipeEntitlement;
using CulinaCloud.Users.Application.RecipeEntitlements.Queries.GetRecipeEntitlement;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.Users.Application.RecipeEntitlements.Queries.GetRecipeEntitlements;
using CulinaCloud.Users.Application.RecipeEntitlements.Commands.UpdateRecipeEntitlement;
using CulinaCloud.Users.Application.RecipeEntitlements.Commands.DeleteRecipeEntitlement;

namespace CulinaCloud.Users.API.Controllers
{
    [Route("users/recipe-entitlements")]
    public class RecipeEntitlementsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<GetRecipeEntitlementsResponse>>> Get([FromQuery] GetRecipeEntitlementsQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetRecipeEntitlementResponse>> Get([FromRoute] Guid id)
        {
            var response = await Mediator.Send(new GetRecipeEntitlementQuery
            {
                Id = id
            });
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CreateRecipeEntitlementResponse>> Create([FromBody] CreateRecipeEntitlementCommand command)
        {
            var response = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new {id = response.Id}, response);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<UpdateRecipeEntitlementResponse>> Update([FromRoute] Guid id, [FromBody] UpdateRecipeEntitlementCommand command)
        {
            command.Id = id;
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<DeleteRecipeEntitlementResponse>> Delete([FromRoute] Guid id)
        {
            var response = await Mediator.Send(new DeleteRecipeEntitlementCommand
            {
                Id = id
            });
            return Ok(response);
        }
    }
}