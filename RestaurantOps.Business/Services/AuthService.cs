using RestaurantOps.Business.Helpers;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data;
using RestaurantOps.Models;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Implements authentication logic including credential validation.
/// </summary>
public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Validates a user's email and password.
    /// </summary>
    public User? ValidateUser(string email, string password)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Email == email);

        if (user == null)
            return null;

        bool isValid = PasswordHelper.VerifyPassword(password, user.PasswordHash);

        return isValid ? user : null;
    }
}