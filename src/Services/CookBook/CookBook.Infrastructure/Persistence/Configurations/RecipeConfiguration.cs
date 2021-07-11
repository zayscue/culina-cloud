using CulinaCloud.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaCloud.CookBook.Infrastructure.Persistence.Configurations
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("Recipes");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(r => r.Name)
                .HasColumnName("Name")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasColumnName("Description")
                .HasMaxLength(8192);

            builder.Property(r => r.EstimatedMinutes)
                .HasColumnName("EstimatedMinutes")
                .IsRequired();

            builder.Property(r => r.Serves)
                .HasColumnName("Serves")
                .HasMaxLength(16);

            builder.Property(r => r.Yield)
                .HasColumnName("Yield")
                .HasMaxLength(16);

            builder.Property(r => r.NumberOfSteps)
                .HasColumnName("NumberOfSteps")
                .IsRequired();

            builder.Property(r => r.NumberOfIngredients)
                .HasColumnName("NumberOfIngredients")
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

            builder.HasOne(r => r.Nutrition)
                .WithOne(n => n.Recipe)
                .HasForeignKey<RecipeNutrition>(n => n.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Ingredients)
                .WithOne(i => i.Recipe)
                .HasForeignKey(i => i.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Images)
                .WithOne(i => i.Recipe)
                .HasForeignKey(i => i.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Metadata)
                .WithOne(m => m.Recipe)
                .HasForeignKey(m => m.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Steps)
                .WithOne(s => s.Recipe)
                .HasForeignKey(s => s.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Tags)
                .WithOne(t => t.Recipe)
                .HasForeignKey(t => t.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
