using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CulinaCloud.EventStore.Domain.Entities;

namespace CulinaCloud.EventStore.Infrastructure.Persistence.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.EventId);

            builder.Property(e => e.EventId)
                .HasColumnName("EventId")
                .IsRequired();

            builder.Property(e => e.EventName)
                .HasColumnName("EventName")
                .IsRequired();

            builder.Property(e => e.AggregateId)
                .HasColumnName("AggregateId")
                .IsRequired();

            builder.Property(e => e.Version)
                .HasColumnName("Version")
                .IsRequired();

            builder.Property(e => e.Data)
                .HasColumnName("Data")
                .IsRequired();

            builder.Property(e => e.Occurred)
                .HasColumnName("Occurred")
                .IsRequired();

            builder.Property(e => e.RaisedBy)
                .HasColumnName("RaisedBy")
                .HasMaxLength(255);

            builder.Property(e => e.Details)
                .HasColumnName("Details")
                .HasMaxLength(255);

            builder.HasOne(e => e.Aggregate)
                .WithMany(a => a.Events);
        }
    }
}
