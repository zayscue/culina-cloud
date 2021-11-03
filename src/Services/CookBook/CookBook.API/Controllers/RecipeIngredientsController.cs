using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.CookBook.Application.RecipeIngredients.Commands.CreateRecipeIngredient;
using CulinaCloud.CookBook.Application.RecipeIngredients.Commands.DeleteRecipeIngredient;
using CulinaCloud.CookBook.Application.RecipeIngredients.Commands.UpdateRecipeIngredient;
using CulinaCloud.CookBook.Application.RecipeIngredients.Queries.GetRecipeIngredient;
using CulinaCloud.CookBook.Application.RecipeIngredients.Queries.GetRecipeIngredients;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.CookBook.API.Controllers
{
    [Route("recipes")]
    public class RecipeIngredientsController : ApiControllerBase
    {
        [HttpGet("{id:guid}/ingredients")]
        public async Task<ActionResult<List<GetRecipeIngredientsResponse>>> Get(Guid id)
        {
            var response = await Mediator.Send(new GetRecipeIngredientsQuery { RecipeId = id });
            return Ok(response);
        }

        [HttpGet("{id:guid}/ingredients/{recipeIngredientId:guid}")]
        public async Task<ActionResult<GetRecipeIngredientResponse>> Get(Guid id, Guid recipeIngredientId)
        {
            var response = await Mediator.Send(new GetRecipeIngredientQuery
            {
                RecipeId = id,
                RecipeIngredientId = recipeIngredientId
            });
            return Ok(response);
        }

        [HttpPost("{id:guid}/ingredients")]
        public async Task<ActionResult<CreateRecipeIngredientResponse>> Create(Guid id, CreateRecipeIngredientCommand command)
        {
            command.RecipeId = id;
            var response = await Mediator.Send(command);
            return CreatedAtAction(
                nameof(Get),
                new
                {
                    id = response.RecipeId,
                    imarecipeIngredientIdgeId = response.Id
                },
                response
            );
        }

        [HttpPut("{id:guid}/ingredients/{recipeIngredientId:guid}")]
        public async Task<ActionResult<UpdateRecipeIngredientResponse>> Update(Guid id, Guid recipeIngredientId, UpdateRecipeIngredientCommand command)
        {
            command.RecipeId = id;
            command.RecipeIngredientId = recipeIngredientId;
            var response = await Mediator.Send(command);
            return Ok(response);
        }


        [HttpDelete("{id:guid}/ingredients/{recipeIngredientId:guid}")]
        public async Task<ActionResult<DeleteRecipeIngredientResponse>> Delete(Guid id, Guid recipeIngredientId)
        {
            var response = await Mediator.Send(new DeleteRecipeIngredientCommand { RecipeId = id, RecipeIngredientId = recipeIngredientId});
            return Ok(response);
        }
    }
}