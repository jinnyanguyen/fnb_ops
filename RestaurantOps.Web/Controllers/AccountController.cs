using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using RestaurantOps.Models;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles user authentication (login/logout).
/// Responsible only for HTTP handling and delegating logic to services.
/// </summary>
public class AccountController : Controller
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Constructor with dependency injection of authentication service.
    /// </summary>
    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Displays login page.
    /// If user is already authenticated, redirect based on role.
    /// </summary>
    public IActionResult Login()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Manager"))
                return RedirectToAction("Index", "Dashboard");

            if (User.IsInRole("Staff"))
                return RedirectToAction("MyTasks", "Task");
        }

        return View();
    }

    /// <summary>
    /// Handles login form submission.
    /// Validates user credentials and signs them in using cookie authentication.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        //  Delegate authentication logic to service layer
        var user = _authService.ValidateUser(model.Email, model.Password);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }

        //  Create claims (identity + authorization + branch context)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("BranchId", user.BranchId.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Sign in user
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal
        );

        // Role-based redirection
        if (user.Role == "Manager")
            return RedirectToAction("Index", "Dashboard");

        return RedirectToAction("MyTasks", "Task");
    }

    /// <summary>
    /// Logs out the currently authenticated user.
    /// Clears authentication cookie.
    /// </summary>
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }

    /// <summary>
    /// Displays access denied page when user lacks permission.
    /// </summary>
    public IActionResult AccessDenied()
    {
        return View();
    }


}