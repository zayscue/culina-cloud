using System;
using MediatR;

namespace Culina.CookBook.Application.Recipes.Queries.GetRecipe
{
    public class GetRecipeQuery : IRequest<GetRecipeResponse>
    {
        public Guid Id { get; set; }
    }
}