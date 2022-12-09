using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.CookBook.Application.RecipeImages.Commands.BatchUpdateRecipeImages;
using CulinaCloud.CookBook.Application.RecipeImages.Commands.CreateRecipeImage;
using CulinaCloud.CookBook.Application.RecipeImages.Commands.DeleteRecipeImage;
using CulinaCloud.CookBook.Application.RecipeImages.Commands.UpdateRecipeImage;
using CulinaCloud.CookBook.Application.RecipeImages.Queries.GetRecipeImage;
using CulinaCloud.CookBook.Application.RecipeImages.Queries.GetRecipeImages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.CookBook.API.Controllers
{
    [Authorize]
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

        [HttpPut("{id:guid}/images")]
        public async Task<ActionResult<List<CreateRecipeImageResponse>>> BatchUpdate(Guid id,
            List<CreateRecipeImageCommand> commands)
        {
            var batchCommand = new BatchUpdateRecipeImagesCommand
            {
                RecipeId = id,
                Commands = commands
            };
            var response = await Mediator.Send(batchCommand);
            return Ok(response);
        }

        [HttpPut("{id:guid}/images/{imageId:guid}")]
        public async Task<ActionResult<UpdateRecipeImageResponse>> Update(Guid id, Guid imageId, UpdateRecipeImageCommand command)
        {
            command.RecipeId = id;
            command.ImageId = imageId;
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id:guid}/images/{imageId:guid}")]
        public async Task<ActionResult<DeleteRecipeImageResponse>> Delete(Guid id, Guid imageId)
        {
            var response = await Mediator.Send(new DeleteRecipeImageCommand { RecipeId = id, ImageId = imageId });
            return Ok(response);
        }
    }
}