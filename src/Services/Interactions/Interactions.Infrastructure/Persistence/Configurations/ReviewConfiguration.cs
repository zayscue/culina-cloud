using CulinaCloud.Interactions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaCloud.Interactions.Infrastructure.Persistence.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.Id);

            builder.HasIndex(r => new { r.UserId, r.RecipeId })
                .IsUnique();

            builder.Property(r => r.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(r => r.RecipeId)
                .HasColumnName("RecipeId")
                .IsRequired();

            builder.Property(r => r.UserId)
                .HasColumnName("UserId")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(r => r.Rating)
                .HasColumnName("Rating")
                .IsRequired();

            builder.Property(r => r.Comments)
                .HasColumnName("Comments")
                .HasMaxLength(1024);

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