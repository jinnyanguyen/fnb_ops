using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Handles authentication logic such as login validation.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Validates user credentials and returns the user if successful.
    /// </summary>
    User? ValidateUser(string email, string password);
}