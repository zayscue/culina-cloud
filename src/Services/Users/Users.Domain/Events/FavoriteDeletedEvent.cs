using CulinaCloud.BuildingBlocks.Common;
using System;

namespace CulinaCloud.Users.Domain.Events
{
    public interface IFavoriteDeleted
    {
        Guid RecipeId { get; set; }
        string UserId { get; set; }
    }

    sealed class FavoriteDeleted : IFavoriteDeleted
    {
        public Guid RecipeId { get; set; }
        public string UserId { get; set; }
    }

    public sealed class FavoriteDeletedEvent : AggregateEventBase<IFavoriteDeleted>
    {
        public override string AggregateType => "Favorite";

        public FavoriteDeletedEvent()
        {
            Data = new FavoriteDeleted();
        }
    }
}
