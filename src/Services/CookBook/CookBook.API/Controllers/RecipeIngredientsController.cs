using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.API.Controllers;
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

        [HttpGet("{id:guid}/ingredients/{ingredientId:guid}")]
        public async Task<ActionResult<GetRecipeIngredientResponse>> Get(Guid id, Guid ingredientId)
        {
            var response = await Mediator.Send(new GetRecipeIngredientQuery { RecipeId = id, IngredientId = ingredientId });
            return Ok(response);
        }
    }
}