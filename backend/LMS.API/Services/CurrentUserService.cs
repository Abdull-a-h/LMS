using System.Security.Claims;
using LMS.Application.Common.Interfaces;
using LMS.Domain.Enums;

namespace LMS.API.Services;

/// <summary>
/// Implements ICurrentUserService over IHttpContextAccessor so Application handlers can read
/// the authenticated caller without depending on ASP.NET.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid? MemberId =>
        Guid.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

    public string? Email => User?.FindFirstValue(ClaimTypes.Email);

    public UserRole? Role =>
        Enum.TryParse<UserRole>(User?.FindFirstValue(ClaimTypes.Role), out var role) ? role : null;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
}
