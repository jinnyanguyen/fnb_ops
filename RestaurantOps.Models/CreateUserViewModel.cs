using System.ComponentModel.DataAnnotations;

namespace RestaurantOps.Web.Models;

/// <summary>
/// View model used for creating a new user.
/// </summary>
public class CreateUserViewModel
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = "Staff";
}