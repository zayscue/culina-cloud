using Culina.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Culina.CookBook.Infrastructure.Persistence.Configurations
{
    public class RecipeTagConfiguration : IEntityTypeConfiguration<RecipeTag>
    {
        public void Configure(EntityTypeBuilder<RecipeTag> builder)
        {
            builder.ToTable("RecipeTags");

            builder.HasKey(r => new {r.RecipeId, r.TagId});

            builder.Property(r => r.RecipeId)
                .HasColumnName("RecipeId")
                .IsRequired();

            builder.Property(r => r.TagId)
                .HasColumnName("TagId")
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

            builder.HasOne(r => r.Tag)
                .WithMany(i => i.RecipeTags)
                .HasForeignKey(r => r.TagId);
        }
    }
}
