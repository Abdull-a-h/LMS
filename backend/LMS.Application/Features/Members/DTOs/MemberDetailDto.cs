using LMS.Application.Features.Borrows.DTOs;

namespace LMS.Application.Features.Members.DTOs;

public record MemberDetailDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public int ActiveBorrowCount { get; init; }
    public IReadOnlyList<BorrowRecordDto> BorrowHistory { get; init; } = Array.Empty<BorrowRecordDto>();
}
