using System;
using MediatR;

namespace CulinaCloud.CookBook.Application.Recipes.Queries.GetRecipe
{
    public class GetRecipeQuery : IRequest<GetRecipeResponse>
    {
        public Guid Id { get; set; }
    }
}