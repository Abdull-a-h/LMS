using LMS.Domain.Common;

namespace LMS.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    // Foreign key + navigation
    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;
}
