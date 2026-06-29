namespace LMS.Domain.Exceptions;

/// <summary>
/// Thrown when authentication fails — wrong email/password, or an invalid, expired, or
/// revoked refresh token. Mapped to HTTP 401 by the API middleware.
/// </summary>
public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string message = "Invalid credentials.") : base(message) { }
}
