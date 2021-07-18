using CulinaCloud.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaCloud.Users.Infrastructure.Persistence.Configurations
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.ToTable("Favorites");

            builder.HasKey(f => new { f.RecipeId, f.UserId });

            builder.Property(f => f.RecipeId)
                .HasColumnName("RecipeId")
                .IsRequired();

            builder.Property(f => f.UserId)
                .HasColumnName("UserId")
                .HasMaxLength(128)
                .IsRequired();

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
