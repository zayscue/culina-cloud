using CulinaCloud.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaCloud.Users.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("ApplicationUsers");

            builder.HasKey(u => u.Id);

            builder.HasIndex(t => t.Email)
                .IsUnique();

            builder.Property(u => u.Id)
                .HasColumnName("Id")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(u => u.DisplayName)
                .HasColumnName("DisplayName")
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("Email")
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(i => i.Picture)
                .HasColumnName("Picture")
                .HasMaxLength(1024)
                .IsRequired();

            builder.Property(u => u.Created)
                .HasColumnName("Created")
                .IsRequired();

            builder.Property(u => u.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(u => u.LastModified)
                .HasColumnName("LastModified");

            builder.Property(u => u.LastModifiedBy)
                .HasColumnName("LastModifiedBy")
                .HasMaxLength(128);
        }
    }
}