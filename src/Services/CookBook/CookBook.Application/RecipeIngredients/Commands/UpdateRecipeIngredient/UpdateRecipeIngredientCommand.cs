using System;
using MediatR;

namespace CulinaCloud.CookBook.Application.RecipeIngredients.Commands.UpdateRecipeIngredient
{
    public class UpdateRecipeIngredientCommand : IRequest<UpdateRecipeIngredientResponse>
    {
        public Guid RecipeId { get; set; }
        public Guid RecipeIngredientId { get; set; }
        public string Quantity { get; set; }
        public string Part { get; set; }
        public string IngredientName { get; set; }
        public string LastModifiedBy { get; set; }
    }
}