using CulinaCloud.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaCloud.CookBook.Infrastructure.Persistence.Configurations
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("Ingredients");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(i => i.IngredientName)
                .HasColumnName("IngredientName")
                .HasMaxLength(128)
                .IsRequired();

            builder.HasIndex(i => i.IngredientName)
                .IsUnique();

            builder.Property(i => i.Created)
                .HasColumnName("Created")
                .IsRequired();

            builder.Property(i => i.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(i => i.LastModified)
                .HasColumnName("LastModified");

            builder.Property(i => i.LastModifiedBy)
                .HasColumnName("LastModifiedBy")
                .HasMaxLength(128);
        }
    }
}
