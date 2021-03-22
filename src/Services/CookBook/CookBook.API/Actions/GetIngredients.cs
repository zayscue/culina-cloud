using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Culina.CookBook.Application.Ingredients.Queries.GetIngredients;

namespace Culina.CookBook.API.Actions
{
    public static class GetIngredients
    {
        public static RequestDelegate Perform = async context =>
        {
            var mediator = context.RequestServices.GetRequiredService<ISender>();
            var query = new GetIngredientsQuery();
            if (context.Request.Query.TryGetValue("name", out var name))
            {
                query.Name = name.ToString();
            }
            if (context.Request.Query.TryGetValue("page", out var page))
            {
                if (int.TryParse(page.ToString(), out var pageInt))
                {
                    query.Page = pageInt;
                }
            }
            if (context.Request.Query.TryGetValue("limit", out var limit))
            {
                if (int.TryParse(limit.ToString(), out var limitInt))
                {
                    query.Limit = limitInt;
                }
            }
            var response = await mediator.Send(query);
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsJsonAsync(response);
        };
    }
}
