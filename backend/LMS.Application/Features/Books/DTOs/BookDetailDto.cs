using LMS.Application.Features.Authors.DTOs;

namespace LMS.Application.Features.Books.DTOs;

public record BookDetailDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string ISBN { get; init; } = string.Empty;
    public int PublicationYear { get; init; }
    public int TotalCopies { get; init; }
    public int AvailableCopies { get; init; }
    public string? CoverImageUrl { get; init; }
    public AuthorDto Author { get; init; } = null!;
    public bool HasActiveBorrowByCurrentMember { get; init; }
}
