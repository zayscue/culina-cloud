using Culina.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Culina.CookBook.Infrastructure.Persistence.Configurations
{
    public class RecipeStepConfiguration : IEntityTypeConfiguration<RecipeStep>
    {
        public void Configure(EntityTypeBuilder<RecipeStep> builder)
        {
            builder.ToTable("RecipeSteps");

            builder.HasKey(r => new { r.RecipeId, r.Order });

            builder.Property(r => r.RecipeId)
                .HasColumnName("RecipeId")
                .IsRequired();

            builder.Property(r => r.Order)
                .HasColumnName("Order")
                .IsRequired();

            builder.Property(r => r.Instruction)
                .HasColumnName("Instruction")
                .HasMaxLength(2048)
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
