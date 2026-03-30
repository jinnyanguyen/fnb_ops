using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Data;
using RestaurantOps.Models;
using RestaurantOps.Business.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles user authentication (login/logout).
/// </summary>
public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Displays login page
    /// </summary>
    public IActionResult Login()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            // Redirect based on role
            if (User.IsInRole("Manager"))
                return RedirectToAction("Index", "Dashboard");

            if (User.IsInRole("Staff"))
                return RedirectToAction("MyTasks", "Task");
        }

        return View();
    }

    /// <summary>
    /// Handles login form submission
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Hash the entered password
        var hashedPassword = PasswordHelper.HashPassword(model.Password);

        // Find matching user
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == model.Email && u.PasswordHash == hashedPassword);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }

        // Create claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // Sign in user
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity)
        );

        // Redirect based on role
        if (user.Role == "Manager")
            return RedirectToAction("Index", "Ingredient");

        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Logs out user
    /// </summary>
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }


}