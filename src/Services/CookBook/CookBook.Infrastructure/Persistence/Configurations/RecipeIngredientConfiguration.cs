using Culina.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Culina.CookBook.Infrastructure.Persistence.Configurations
{
    public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {
            builder.ToTable("RecipeIngredients");

            builder.HasKey(r => new {r.RecipeId, r.IngredientId});

            builder.Property(r => r.RecipeId)
                .HasColumnName("RecipeId")
                .IsRequired();

            builder.Property(r => r.IngredientId)
                .HasColumnName("Ingredient")
                .IsRequired();

            builder.Property(r => r.Quantity)
                .HasColumnName("Quantity")
                .HasMaxLength(16)
                .IsRequired();

            builder.Property(r => r.Part)
                .HasColumnName("Part")
                .HasMaxLength(64)
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

            builder.HasOne(r => r.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(r => r.IngredientId);
        }
    }
}
