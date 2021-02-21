using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Text.Json;
using Xunit;

namespace CulinaCloud.BuildingBlocks.Common.UnitTests
{
    public class TestAggregateCreatedEvent : AggregateDomainEvent
    {
        public override string AggregateType => "TestAggregate";

        public string Description { get; set; }
    }

    public class AggregateDomainEventTests
    {
        [Fact]
        public void ShouldHaveAnId()
        {
            var testAggregateDescription = "This is a test!";
            var testEvent = new TestAggregateCreatedEvent()
            {
                Description = testAggregateDescription
            };
            testEvent.EventId.Should().NotBeEmpty();
        }

        [Fact]
        public void ShouldHaveAnAggregateId()
        {
            var testAggregateDescription = "This is a test!";
            var testEvent = new TestAggregateCreatedEvent()
            {
                Description = testAggregateDescription
            };
            testEvent.AggregateId.Should().NotBeEmpty();
        }

        [Fact]
        public void ShouldHaveAnAggregateType()
        {
            var testAggregateDescription = "This is a test!";
            var testEvent = new TestAggregateCreatedEvent()
            {
                Description = testAggregateDescription
            };
            testEvent.AggregateType.Should().Be("TestAggregate");
        }

        [Fact]
        public void ShouldHaveData()
        {
            var testAggregateDescription = "This is a test!";
            var testEvent = new TestAggregateCreatedEvent()
            {
                Description = testAggregateDescription
            };

            var data = testEvent.Data();
            data["Description"].Should().Be(testAggregateDescription);
        }

        [Fact]
        public void ShouldSerialize()
        {
            var testAggregateDescription = "This is a test!";
            var testEvent = new TestAggregateCreatedEvent()
            {
                Description = testAggregateDescription
            };

            var jsonString = testEvent.Serialize();

            using JsonDocument document = JsonDocument.Parse(jsonString);
            JsonElement root = document.RootElement;
            JsonElement dataElement = root.GetProperty("data");
            var description = dataElement.GetProperty("Description").GetString();
            description.Should().Be(testAggregateDescription);
        }
    }
}
