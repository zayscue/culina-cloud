using Culina.CookBook.Application.Tags.Commands.CreateTag;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Culina.CookBook.API.Actions
{
    public static class PostTags
    {
        public static readonly RequestDelegate Perform = async context =>
        {
            var mediator = context.RequestServices.GetRequiredService<ISender>();
            var command = await context.Request.ReadFromJsonAsync<CreateTagCommand>();
            var response = await mediator.Send(command!);
            context.Response.StatusCode = StatusCodes.Status201Created;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response);
        };
    }
}