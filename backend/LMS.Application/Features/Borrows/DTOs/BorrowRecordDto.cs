namespace LMS.Application.Features.Borrows.DTOs;

public record BorrowRecordDto
{
    public Guid Id { get; init; }
    public Guid BookId { get; init; }
    public string BookTitle { get; init; } = string.Empty;
    public DateTime BorrowedAt { get; init; }
    public DateTime DueDate { get; init; }
    public DateTime? ReturnedAt { get; init; }
    public bool IsOverdue { get; init; }
}
