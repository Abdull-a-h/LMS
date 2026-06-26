namespace LMS.Application.Features.Authors.DTOs;

public record AuthorDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Biography { get; init; }
    public string? Nationality { get; init; }
}
