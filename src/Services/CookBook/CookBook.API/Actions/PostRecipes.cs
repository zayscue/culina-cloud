using System.Text.Json;
using System.Text.Json.Serialization;
using Culina.CookBook.Application.Recipes.Commands.CreateRecipe;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Culina.CookBook.API.Actions
{
    public static class PostRecipes
    {
        public static readonly RequestDelegate Perform = async context =>
        {
            var mediator = context.RequestServices.GetRequiredService<ISender>();
            var createIngredientCommand = await context.Request.ReadFromJsonAsync<CreateRecipeCommand>();
            var response = await mediator.Send(createIngredientCommand!);
            context.Response.StatusCode = StatusCodes.Status201Created;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            });
        };
    }
}