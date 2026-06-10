using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantOps.Business.Helpers;
using RestaurantOps.Data;
using RestaurantOps.Models;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles user management operations.
/// Only managers can manage users.
/// </summary>
[Authorize(Roles = "Manager")]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Displays all users.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var users = await _context.Users
            .Include(u => u.Branch)
            .ToListAsync();

        return View(users);
    }

    /// <summary>
    /// Displays create user form.
    /// </summary>
    public async Task<IActionResult> Create()
    {
        ViewBag.Branches = await _context.Branches.ToListAsync();

        return View();
    }

    /// <summary>
    /// Handles create user submission.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User user, string password)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Branches = await _context.Branches.ToListAsync();
                return View(user);
            }

            // Check duplicate email
            var exists = await _context.Users
                .AnyAsync(u => u.Email == user.Email);

            if (exists)
            {
                ModelState.AddModelError("", "Email already exists.");

                ViewBag.Branches = await _context.Branches.ToListAsync();

                return View(user);
            }

            // Hash password
            user.PasswordHash = PasswordHelper.HashPassword(password);

            // Set created date
            user.CreatedAt = DateTime.UtcNow;

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            TempData["Success"] = "User created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            ViewBag.Branches = await _context.Branches.ToListAsync();

            return View(user);
        }
    }
}