using System;
using CulinaCloud.BuildingBlocks.Common;

namespace CulinaCloud.Interactions.Domain.Events
{
    public interface IReviewCreated
    {
        Guid Id { get; set; }
        Guid RecipeId { get; set; }
        string UserId { get; set; }
        int Rating { get; set; }
        string Comments { get; set; }
    }

    sealed class ReviewCreated : IReviewCreated
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }

    public sealed class ReviewCreatedEvent : AggregateEventBase<IReviewCreated>
    {
        public override string AggregateType => "Review";

        public ReviewCreatedEvent()
        {
            Data = new ReviewCreated();
        }
    }
}
