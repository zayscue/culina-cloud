using CulinaCloud.Analytics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaCloud.Analytics.Infrastructure.Persistence.Configurations
{
    public class RecipePopularityConfiguration : IEntityTypeConfiguration<RecipePopularity>
    {
        public void Configure(EntityTypeBuilder<RecipePopularity> builder)
        {
            builder.ToTable("RecipePopularity");

            builder.HasKey(r => r.RecipeId);

            builder.Property(r => r.RecipeId)
                .HasColumnName("RecipeId")
                .IsRequired();

            builder.Property(r => r.Submitted)
                .HasColumnName("Submitted")
                .HasColumnType("DATE")
                .IsRequired();

            builder.Property(r => r.RatingCount)
                .HasColumnName("RatingCount")
                .IsRequired();

            builder.Property(r => r.RatingSum)
                .HasColumnName("RatingSum")
                .IsRequired();

            builder.Property(r => r.RatingAverage)
                .HasColumnName("RatingAverage")
                .HasColumnType("decimal(18, 16)")
                .IsRequired();

            builder.Property(r => r.RatingWeightedAverage)
                .HasColumnName("RatingWeightedAverage")
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
