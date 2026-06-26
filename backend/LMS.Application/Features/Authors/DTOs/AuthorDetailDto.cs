namespace LMS.Application.Features.Authors.DTOs;

public record AuthorDetailDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Biography { get; init; }
    public string? Nationality { get; init; }
    public IReadOnlyList<BookSummaryDto> Books { get; init; } = Array.Empty<BookSummaryDto>();
}
