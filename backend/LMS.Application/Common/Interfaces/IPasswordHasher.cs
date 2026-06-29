namespace LMS.Application.Common.Interfaces;

/// <summary>
/// Abstracts password hashing so the Application layer never references a hashing library
/// (the BCrypt implementation lives in Infrastructure).
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string passwordHash);
}
