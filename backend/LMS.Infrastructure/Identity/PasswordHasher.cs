using LMS.Application.Common.Interfaces;

namespace LMS.Infrastructure.Identity;

/// <summary>BCrypt-based password hashing. BCrypt embeds a per-password salt in the hash string.</summary>
public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
