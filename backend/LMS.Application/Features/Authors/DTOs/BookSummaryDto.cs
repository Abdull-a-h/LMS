namespace LMS.Application.Features.Authors.DTOs;

public record BookSummaryDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string ISBN { get; init; } = string.Empty;
}
