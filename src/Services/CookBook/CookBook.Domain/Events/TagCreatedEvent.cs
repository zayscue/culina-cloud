using System;
using CulinaCloud.BuildingBlocks.Common;

namespace Culina.CookBook.Domain.Events
{
    public interface ITagCreated
    {
        Guid Id { get; set; }
        string TagName { get; set; }
    }

    sealed class TagCreated : ITagCreated
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
    }

    public sealed class TagCreatedEvent : AggregateEventBase<ITagCreated>
    {
        public override string AggregateType => "RecipeTag";

        public TagCreatedEvent()
        {
            Data = new TagCreated();
        }
    }
}