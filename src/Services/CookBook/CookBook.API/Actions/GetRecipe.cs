using System;
using Culina.CookBook.Application.Recipes.Queries.GetRecipe;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Culina.CookBook.API.Actions
{
    public static class GetRecipe
    {
        public static readonly RequestDelegate Perform = async context =>
        {
            var mediator = context.RequestServices.GetRequiredService<ISender>();
            var recipeId = new Guid(context.Request.RouteValues["recipeId"].ToString());
            var query = new GetRecipeQuery()
            {
                Id = recipeId
            };
            var response = await mediator.Send(query);
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsJsonAsync(response);
        };
    }
}