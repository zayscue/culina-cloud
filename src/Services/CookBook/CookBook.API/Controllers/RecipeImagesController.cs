using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.CookBook.Application.RecipeImages.Commands.CreateRecipeImage;
using CulinaCloud.CookBook.Application.RecipeImages.Commands.DeleteRecipeImage;
using CulinaCloud.CookBook.Application.RecipeImages.Queries.GetRecipeImage;
using CulinaCloud.CookBook.Application.RecipeImages.Queries.GetRecipeImages;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.CookBook.API.Controllers
{
    [Route("recipes")]
    public class RecipeImagesController : ApiControllerBase
    {
        [HttpGet("{id:guid}/images")]
        public async Task<ActionResult<List<GetRecipeImagesResponse>>> Get(Guid id)
        {
            var response = await Mediator.Send(new GetRecipeImagesQuery { RecipeId = id });
            return Ok(response);
        }

        [HttpGet("{id:guid}/images/{imageId:guid}")]
        public async Task<ActionResult<GetRecipeImageResponse>> Get(Guid id, Guid imageId)
        {
            var response = await Mediator.Send(new GetRecipeImageQuery { RecipeId = id, ImageId = imageId});
            return Ok(response);
        }

        [HttpPost("{id:guid}/images")]
        public async Task<ActionResult<CreateRecipeImageResponse>> Create(Guid id, CreateRecipeImageCommand command)
        {
            command.RecipeId = id;
            var response = await Mediator.Send(command);
            return CreatedAtAction(
                nameof(Get),
                new
                {
                    id = response.RecipeId,
                    imageId = response.ImageId
                },
                response
            );
        }

        [HttpDelete("{id:guid}/images/{imageId:guid}")]
        public async Task<ActionResult<DeleteRecipeImageResponse>> Delete(Guid id, Guid imageId)
        {
            var response = await Mediator.Send(new DeleteRecipeImageCommand { RecipeId = id, ImageId = imageId });
            return Ok(response);
        }
    }
}