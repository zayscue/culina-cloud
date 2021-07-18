using CulinaCloud.BuildingBlocks.Common;
using System;

namespace CulinaCloud.Users.Domain.Events
{
    public interface IFavoriteCreated
    {
        Guid RecipeId { get; set; }
        string UserId { get; set; }
    }

    sealed class FavoriteCreated : IFavoriteCreated
    {
        public Guid RecipeId { get; set; }
        public string UserId { get; set; }
    }

    public sealed class FavoriteCreatedEvent : AggregateEventBase<IFavoriteCreated>
    {
        public override string AggregateType => "Favorite";

        public FavoriteCreatedEvent()
        {
            Data = new FavoriteCreated();
        }
    }
}
