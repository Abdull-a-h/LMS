using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Token)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(t => t.ExpiresAt)
            .IsRequired();

        // RevokedAt is nullable: null means the token is still valid.
        builder.Property(t => t.RevokedAt);

        // Tokens are looked up by their value during refresh/logout.
        builder.HasIndex(t => t.Token);

        // The Member relationship is configured from MemberConfiguration (cascade delete).
    }
}
