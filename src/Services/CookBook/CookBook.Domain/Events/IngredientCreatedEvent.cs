using System;
using CulinaCloud.BuildingBlocks.Common;

namespace Culina.CookBook.Domain.Events
{
    public interface IIngredientCreated
    {
        Guid IngredientId { get; set; }
        string IngredientName { get; set; }
    }

    sealed class EventData : IIngredientCreated
    {
        public Guid IngredientId { get; set;  }
        public string IngredientName { get; set; }
    }

    public sealed class IngredientCreatedEvent : AggregateEventBase<IIngredientCreated>
    {
        public override string AggregateType => "Ingredient";

        public IngredientCreatedEvent()
        {
            Data = new EventData();
        }
    }
}