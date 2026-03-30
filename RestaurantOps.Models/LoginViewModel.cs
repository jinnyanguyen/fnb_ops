using System.ComponentModel.DataAnnotations;

namespace RestaurantOps.Models;

/// <summary>
/// Represents login form input.
/// </summary>
public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}