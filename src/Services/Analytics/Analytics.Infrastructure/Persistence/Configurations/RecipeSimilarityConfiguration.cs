using CulinaCloud.Analytics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaCloud.Analytics.Infrastructure.Persistence.Configurations
{
    public class RecipeSimilarityConfiguration : IEntityTypeConfiguration<RecipeSimilarity>
    {
        public void Configure(EntityTypeBuilder<RecipeSimilarity> builder)
        {
            builder.ToTable("RecipeSimilarity");

            builder.HasKey(r => new { r.RecipeId, r.SimilarRecipeId, r.SimilarityType });

            builder.Property(r => r.RecipeId)
                .HasColumnName("RecipeId")
                .IsRequired();

            builder.Property(r => r.SimilarRecipeId)
                .HasColumnName("SimilarRecipeId")
                .IsRequired();

            builder.Property(r => r.SimilarityType)
                .HasColumnName("SimilarityType")
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(r => r.SimilarityScore)
                .HasColumnName("SimilarityScore")
                .HasColumnType("decimal(18, 16)")
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
