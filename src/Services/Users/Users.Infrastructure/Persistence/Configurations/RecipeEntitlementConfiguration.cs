using System;
using CulinaCloud.Users.Domain.Entities;
using CulinaCloud.Users.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaCloud.Users.Infrastructure.Persistence.Configurations
{
    public class RecipeEntitlementConfiguration : IEntityTypeConfiguration<RecipeEntitlement>
    {
        public void Configure(EntityTypeBuilder<RecipeEntitlement> builder)
        {
            builder.ToTable("RecipeEntitlements");

            builder.HasKey(e => e.Id);

            builder.HasIndex(e => new { e.UserId, e.RecipeId })
                .IsUnique();

            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(e => e.UserId)
                .HasColumnName("UserId")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(e => e.RecipeId)
                .HasColumnName("RecipeId")
                .IsRequired();

            builder.Property(e => e.Type)
                .HasColumnName("Type")
                .HasConversion(
                    e => e.ToString(),
                    e => (RecipeEntitlementType)Enum.Parse(typeof(RecipeEntitlementType), e)
                ).IsRequired();

            builder.Property(f => f.Created)
                .HasColumnName("Created")
                .IsRequired();

            builder.Property(f => f.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(f => f.LastModified)
                .HasColumnName("LastModified");

            builder.Property(f => f.LastModifiedBy)
                .HasColumnName("LastModifiedBy")
                .HasMaxLength(128);
        }
    }
}