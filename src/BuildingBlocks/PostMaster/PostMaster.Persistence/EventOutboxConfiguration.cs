using CulinaCloud.BuildingBlocks.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaCloud.BuildingBlocks.PostMaster.Persistence
{
    public class EventOutboxConfiguration : IEntityTypeConfiguration<AggregateEventEntity>
    {
        public void Configure(EntityTypeBuilder<AggregateEventEntity> builder)
        {
            builder.ToTable("EventOutbox");

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

            builder.Property(e => e.AggregateType)
                .HasColumnName("AggregateType")
                .IsRequired();

            builder.Property(e => e.RaisedBy)
                .HasColumnName("RaisedBy")
                .HasMaxLength(255);

            builder.Property(e => e.Details)
                .HasColumnName("Details")
                .HasMaxLength(255);

            builder.Property(e => e.Occurred)
                .HasColumnName("Occurred")
                .IsRequired();

            builder.Property(e => e.Data)
                .HasColumnName("Data")
                .IsRequired();

            builder.Property(e => e.IsPublished)
                .HasColumnName("IsPublished")
                .IsRequired();

            builder.Property(e => e.IsStored)
                .HasColumnName("IsStored")
                .IsRequired();
        }
    }
}
