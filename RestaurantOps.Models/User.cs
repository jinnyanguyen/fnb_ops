using System.ComponentModel.DataAnnotations;

namespace RestaurantOps.Models;

/// <summary>
/// Represents an application user (Manager or Staff).
/// This entity is used for authentication and role-based authorization.
/// </summary>
public class User
{
    /// <summary>
    /// Primary key for the User table.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// User's first name.
    /// </summary>
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name.
    /// </summary>
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Unique email used for login.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Stores hashed password (never plain text).
    /// </summary>
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Role of the user: "Manager" or "Staff".
    /// Determines access level.
    /// </summary>
    [Required]
    public string Role { get; set; } = "Staff";
}