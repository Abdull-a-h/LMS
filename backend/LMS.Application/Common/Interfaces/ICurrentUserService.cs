using LMS.Domain.Enums;

namespace LMS.Application.Common.Interfaces;

/// <summary>
/// Exposes the authenticated user from the current request (implemented in the API layer
/// over IHttpContextAccessor). Lets handlers identify the caller without referencing ASP.NET.
/// </summary>
public interface ICurrentUserService
{
    Guid? MemberId { get; }
    string? Email { get; }
    UserRole? Role { get; }
    bool IsAuthenticated { get; }
}
