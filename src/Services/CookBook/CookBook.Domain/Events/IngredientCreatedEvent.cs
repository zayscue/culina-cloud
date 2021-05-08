using Culina.CookBook.Domain.Entities;
using CulinaCloud.BuildingBlocks.Common;

namespace Culina.CookBook.Domain.Events
{
    public class IngredientCreatedEvent : AggregateEvent
    {
        public override string AggregateType => "Ingredient";

        public IngredientCreatedEvent(Ingredient ingredient)
        {
            AggregateId = ingredient.Id;
            Data = new
            {
                ingredient.Id,
                ingredient.IngredientName
            };
        }
    }
}