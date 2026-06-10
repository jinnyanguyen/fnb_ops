using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    /// <summary>
    /// Foreign key to the branch this user belongs to.
    /// </summary>
    [Required]
    public int BranchId { get; set; }

    /// <summary>
    /// Navigation property to the user's branch.
    /// </summary>
    [ForeignKey(nameof(BranchId))]
    public Branch? Branch { get; set; }

    /// <summary>
    /// Convenience property for displaying the user's full name in UI views.
    /// </summary>
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Date user account was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

