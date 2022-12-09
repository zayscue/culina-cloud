using System;
using MediatR;

namespace CulinaCloud.CookBook.Application.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommand : IRequest<UpdateRecipeResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EstimatedMinutes { get; set; }
        public string Serves { get; set; }
        public string Yield { get; set; }

        public string LastModifiedBy { get; set; }
    }
}