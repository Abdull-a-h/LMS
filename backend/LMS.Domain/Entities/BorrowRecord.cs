using LMS.Domain.Common;

namespace LMS.Domain.Entities;

public class BorrowRecord : BaseEntity
{
    public DateTime BorrowedAt { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedAt { get; set; }

    // Foreign keys + navigation
    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;
}
