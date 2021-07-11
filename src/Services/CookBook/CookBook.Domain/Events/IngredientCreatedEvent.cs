using System;
using CulinaCloud.BuildingBlocks.Common;

namespace CulinaCloud.CookBook.Domain.Events
{
    public interface IIngredientCreated
    {
        Guid Id { get; set; }
        string IngredientName { get; set; }
    }

    sealed class IngredientCreated : IIngredientCreated
    {
        public Guid Id { get; set;  }
        public string IngredientName { get; set; }
    }

    public sealed class IngredientCreatedEvent : AggregateEventBase<IIngredientCreated>
    {
        public override string AggregateType => "Ingredient";

        public IngredientCreatedEvent()
        {
            Data = new IngredientCreated();
        }
    }
}