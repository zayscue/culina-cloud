using CulinaCloud.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaCloud.CookBook.Infrastructure.Persistence.Configurations
{
    public class RecipeNutritionConfiguration : IEntityTypeConfiguration<RecipeNutrition>
    {
        public void Configure(EntityTypeBuilder<RecipeNutrition> builder)
        {
            builder.ToTable("RecipeNutrition");

            builder.HasKey(r => r.RecipeId);

            builder.Property(r => r.RecipeId)
                .HasColumnName("RecipeId")
                .IsRequired();

            builder.Property(r => r.ServingSize)
                .HasColumnName("ServingSize")
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(r => r.ServingsPerRecipe)
                .HasColumnName("ServingsPerRecipe")
                .IsRequired();

            builder.Property(r => r.Calories)
                .HasColumnName("Calories")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.CaloriesFromFat)
                .HasColumnName("CaloriesFromFat")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.CaloriesFromFatPdv)
                .HasColumnName("CaloriesFromFatPdv")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.TotalFat)
                .HasColumnName("TotalFat")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.TotalFatPdv)
                .HasColumnName("TotalFatPdv")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.SaturatedFat)
                .HasColumnName("SaturatedFat")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.SaturatedFatPdv)
                .HasColumnName("SaturatedFatPdv")
                .HasColumnName("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.Cholesterol)
                .HasColumnName("Cholesterol")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.CholesterolPdv)
                .HasColumnName("CholesterolPdv")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.DietaryFiber)
                .HasColumnName("DietaryFiber")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.DietaryFiberPdv)
                .HasColumnName("DietaryFiberPdv")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.Sugar)
                .HasColumnName("Sugar")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.SugarPdv)
                .HasColumnName("SugarPdv")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.Sodium)
                .HasColumnName("Sodium")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.SodiumPdv)
                .HasColumnName("SodiumPdv")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.Protein)
                .HasColumnName("Protein")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.ProteinPdv)
                .HasColumnName("ProteinPdv")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.TotalCarbohydrates)
                .HasColumnName("TotalCarbohydrates")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();

            builder.Property(r => r.TotalCarbohydratesPdv)
                .HasColumnName("TotalCarbohydratesPdv")
                .HasColumnType("decimal(8, 2)")
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
