using Culina.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Culina.CookBook.Infrastructure.Persistence.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(t => t.TagName)
                .HasColumnName("TagName")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(t => t.Created)
                .HasColumnName("Created")
                .IsRequired();

            builder.Property(t => t.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(t => t.LastModified)
                .HasColumnName("LastModified");

            builder.Property(t => t.LastModifiedBy)
                .HasColumnName("LastModifiedBy")
                .HasMaxLength(128);
        }
    }
}
