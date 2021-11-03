using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.CookBook.Application.RecipeTags.Queries.GetRecipeTag;
using CulinaCloud.CookBook.Application.RecipeTags.Queries.GetRecipeTags;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.CookBook.API.Controllers
{
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
    }
}