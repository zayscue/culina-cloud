using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Culina.CookBook.Application.Ingredients.Commands.CreateIngredient;

namespace Culina.CookBook.API.Actions
{
    public static class PostIngredient
    {
        public static readonly RequestDelegate Perform = async context =>
        {
            var mediator = context.RequestServices.GetRequiredService<ISender>();
            var createIngredientCommand = await context.Request.ReadFromJsonAsync<CreateIngredientCommand>();
            var response = await mediator.Send(createIngredientCommand!);
            context.Response.StatusCode = StatusCodes.Status201Created;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response);
        };
    }
}
