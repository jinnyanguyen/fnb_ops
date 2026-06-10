using System.Security.Cryptography;
using System.Text;

namespace RestaurantOps.Business.Helpers;

/// <summary>
/// Provides password hashing and verification functionality.
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

    /// <summary>
    /// Verifies if an input password matches the stored hash.
    /// </summary>
    public static bool VerifyPassword(string inputPassword, string storedHash)
    {
        var inputHash = HashPassword(inputPassword);
        return inputHash == storedHash;
    }
}