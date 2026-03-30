using System.Security.Cryptography;
using System.Text;

namespace RestaurantOps.Business.Helpers;

/// <summary>
/// Provides helper methods for hashing passwords.
/// </summary>
public static class PasswordHelper
{
    /// <summary>
    /// Hashes a plain text password using SHA256.
    /// </summary>
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();

        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }
}