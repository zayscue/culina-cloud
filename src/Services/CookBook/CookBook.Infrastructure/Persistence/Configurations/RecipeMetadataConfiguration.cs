using CulinaCloud.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaCloud.CookBook.Infrastructure.Persistence.Configurations
{
    public class RecipeMetadataConfiguration : IEntityTypeConfiguration<RecipeMetadata>
    {
        public void Configure(EntityTypeBuilder<RecipeMetadata> builder)
        {
            builder.ToTable("RecipeMetadata");

            builder.HasKey(r => new {r.RecipeId, r.Type});

            builder.Property(r => r.RecipeId)
                .HasColumnName("RecipeId")
                .IsRequired();

            builder.Property(r => r.Type)
                .HasColumnName("Type")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(r => r.Value)
                .HasColumnName("Value")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(r => r.Created)
                .HasColumnName("Created")
                .IsRequired();

            builder.Property(r => r.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(r => r.LastModified)
                .HasColumnName("LastModified");

            builder.Property(r => r.LastModifiedBy)
                .HasColumnName("LastModifiedBy")
                .HasMaxLength(128);
        }
    }
}
