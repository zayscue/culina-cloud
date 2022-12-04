using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.CookBook.Application.RecipeTags.Commands.BatchUpdateRecipeTags;
using CulinaCloud.CookBook.Application.RecipeTags.Commands.CreateRecipeTag;
using CulinaCloud.CookBook.Application.RecipeTags.Commands.DeleteRecipeTag;
using CulinaCloud.CookBook.Application.RecipeTags.Queries.GetRecipeTag;
using CulinaCloud.CookBook.Application.RecipeTags.Queries.GetRecipeTags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.CookBook.API.Controllers
{
    [Authorize]
    [Route("recipes")]
    public class RecipeTagsController : ApiControllerBase
    {
        [HttpGet("{id:guid}/tags")]
        public async Task<ActionResult<List<GetRecipeTagsResponse>>> Get(Guid id)
        {
            var response = await Mediator.Send(new GetRecipeTagsQuery { RecipeId = id });
            return Ok(response);
        }

        [HttpGet("{id:guid}/tags/{tagId:guid}")]
        public async Task<ActionResult<GetRecipeTagResponse>> Get(Guid id, Guid tagId)
        {
            var response = await Mediator.Send(new GetRecipeTagQuery { RecipeId = id, TagId = tagId });
            return Ok(response);
        }

        [HttpPost("{id:guid}/tags")]
        public async Task<ActionResult<CreateRecipeTagResponse>> Create(Guid id, CreateRecipeTagCommand command)
        {
            command.RecipeId = id;
            var response = await Mediator.Send(command);
            return CreatedAtAction(
                nameof(Get),
                new
                {
                    id = response.RecipeId,
                    tagId = response.TagId
                },
                response
            );
        }

        [HttpPut("{id:guid}/tags")]
        public async Task<ActionResult<List<CreateRecipeTagResponse>>> BatchUpdate(Guid id,
            List<CreateRecipeTagCommand> commands)
        {
            var command = new BatchUpdateRecipeTagsCommand
            {
                RecipeId = id,
                Commands = commands
            };
            var response = await Mediator.Send(command);
            return response;
        }

        [HttpDelete("{id:guid}/tags/{tagId:guid}")]
        public async Task<ActionResult<DeleteRecipeTagResponse>> Delete(Guid id, Guid tagId)
        {
            var response = await Mediator.Send(new DeleteRecipeTagCommand { RecipeId = id, TagId = tagId });
            return Ok(response);
        }
    }
}