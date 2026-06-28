using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.FullName)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(m => m.PasswordHash)
            .IsRequired()
            .HasMaxLength(256);

        // Store the enum as a readable string ("Member"/"Librarian") instead of an int.
        builder.Property(m => m.Role)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(m => m.IsActive)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        // Email must be unique across all members.
        builder.HasIndex(m => m.Email).IsUnique();

        // Deactivated members are hidden from every query automatically (soft delete).
        builder.HasQueryFilter(m => m.IsActive);

        // One member has many borrow records and many refresh tokens.
        builder.HasMany(m => m.BorrowRecords)
            .WithOne(b => b.Member)
            .HasForeignKey(b => b.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.RefreshTokens)
            .WithOne(t => t.Member)
            .HasForeignKey(t => t.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
