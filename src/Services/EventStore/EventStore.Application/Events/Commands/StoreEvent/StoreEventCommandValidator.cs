using System;
using FluentValidation;

namespace CulinaCloud.EventStore.Application.Events.Commands.StoreEvent
{
    public class StoreEventCommandValidator : AbstractValidator<StoreEventCommand>
    {
        public StoreEventCommandValidator()
        {
            RuleFor(c => c.AggregateId)
                .NotEmpty();

            RuleForEach(c => c.Events).ChildRules(e => {
                e.RuleFor(x => x.AggregateType).NotEmpty().NotNull();
                e.RuleFor(x => x.Data).NotNull();
                e.RuleFor(x => x.EventId).NotEmpty();
                e.RuleFor(x => x.Occurred).NotNull().LessThan(DateTimeOffset.UtcNow);
                e.RuleFor(x => x.EventName).NotNull().NotEmpty();
                e.RuleFor(x => x.RaisedBy).MaximumLength(255);
                e.RuleFor(x => x.Details).MaximumLength(255);
            });
        }
    }
}
