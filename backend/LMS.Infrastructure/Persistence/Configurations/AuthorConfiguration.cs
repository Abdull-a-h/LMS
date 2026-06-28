using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.Biography)
            .HasMaxLength(1000);

        builder.Property(a => a.Nationality)
            .HasMaxLength(100);

        builder.Property(a => a.IsActive)
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        // Soft delete: inactive authors are hidden from every query automatically.
        builder.HasQueryFilter(a => a.IsActive);

        // The Book -> Author relationship (with Restrict) is configured from BookConfiguration.
    }
}
