using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CulinaCloud.EventStore.Domain.Entities;

namespace CulinaCloud.EventStore.Infrastructure.Persistence.Configurations
{
    public class AggregateConfiguration : IEntityTypeConfiguration<Aggregate>
    {
        public void Configure(EntityTypeBuilder<Aggregate> builder)
        {
            builder.HasKey(a => a.AggregateId);

            builder.Property(a => a.AggregateId)
                .HasColumnName("AggregateId")
                .IsRequired();

            builder.Property(a => a.AggregateType)
                .HasColumnName("AggregateType")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Version)
                .HasColumnName("Version")
                .IsRequired();
        }
    }
}
