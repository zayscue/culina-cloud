using Culina.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Culina.CookBook.Infrastructure.Persistence.Configurations
{
    public class RecipeImageConfiguration : IEntityTypeConfiguration<RecipeImage>
    {
        public void Configure(EntityTypeBuilder<RecipeImage> builder)
        {
            builder.ToTable("RecipeImages");

            builder.HasKey(r => new { r.RecipeId, r.ImageId });

            builder.Property(r => r.RecipeId)
                .HasColumnName("RecipeId")
                .IsRequired();

            builder.Property(r => r.ImageId)
                .HasColumnName("ImageId")
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
            
            builder.HasOne(r => r.Image)
                .WithMany(i => i.RecipeImages)
                .HasForeignKey(r => r.ImageId);
        }
    }
}
