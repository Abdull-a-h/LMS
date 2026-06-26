namespace LMS.Application.Features.Books.DTOs;

public record BookDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string ISBN { get; init; } = string.Empty;
    public int PublicationYear { get; init; }
    public int TotalCopies { get; init; }
    public int AvailableCopies { get; init; }
    public string? CoverImageUrl { get; init; }
    public Guid AuthorId { get; init; }
    public string AuthorName { get; init; } = string.Empty;
}
