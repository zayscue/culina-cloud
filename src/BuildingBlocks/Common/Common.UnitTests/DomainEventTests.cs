using FluentAssertions;
using System;
using System.Text.Json;
using Xunit;

namespace CulinaCloud.BuildingBlocks.Common.UnitTests
{
    public class BasicTestDomainEvent : DomainEvent {}

    public class ComplexTestDomainEvent: DomainEvent
    {
        public ComplexTestDomainEvent()
        {
            Details = "This is an event for testing purposes";
        }
    }

    public class DomainEventTests
    {
        [Fact]
        public void ShouldHaveIdAndOccurredTimestamp()
        {
            var testEvent = new BasicTestDomainEvent();
            testEvent.EventId.Should().NotBeEmpty();
            testEvent.Occurred.Should().NotBeAfter(DateTime.UtcNow);
            testEvent.IsPublished.Should().BeFalse();
            testEvent.RaisedBy.Should().BeNull();
            testEvent.Details.Should().BeNull();
        }

        [Fact]
        public void CanHaveRaisedByAndDetailsToDescribeTheEvent()
        {
            var testUserId = Guid.NewGuid().ToString();
            var testEvent = new ComplexTestDomainEvent
            {
                RaisedBy = testUserId
            };
            testEvent.EventId.Should().NotBeEmpty();
            testEvent.Occurred.Should().NotBeAfter(DateTime.UtcNow);
            testEvent.IsPublished.Should().BeFalse();
            testEvent.RaisedBy.Should().Be(testUserId);
            testEvent.Details.Should().Be("This is an event for testing purposes");
        }
    }
}
