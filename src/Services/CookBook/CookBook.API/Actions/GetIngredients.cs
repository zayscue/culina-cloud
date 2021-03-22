using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Culina.CookBook.API.Actions
{
    public static class GetIngredients
    {
        public static RequestDelegate Perform = async context =>
        {
            var mediator = context.RequestServices.GetRequiredService<ISender>();
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync("Not Implemented Yet");
        };
    }
}
