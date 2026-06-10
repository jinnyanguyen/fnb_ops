using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data;
using RestaurantOps.Models;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles task assignment and task updates.
/// Supports branch-aware task isolation.
/// </summary>
[Authorize]
public class TaskController : Controller
{
    private readonly ITaskService _taskService;
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public TaskController(
        ITaskService taskService,
        ApplicationDbContext context)
    {
        _taskService = taskService;
        _context = context;
    }

    /// <summary>
    /// Manager view:
    /// Displays all tasks for the manager's branch.
    /// </summary>
    [Authorize(Roles = "Manager")]
    public IActionResult Index()
    {
        var branchIdClaim = User.FindFirst("BranchId")?.Value;

        if (string.IsNullOrEmpty(branchIdClaim))
        {
            return Unauthorized();
        }

        int branchId = int.Parse(branchIdClaim);

        var tasks = _taskService.GetAll(branchId);

        return View(tasks);
    }

    /// <summary>
    /// Staff view:
    /// Displays only tasks assigned to the logged-in user.
    /// </summary>
    public IActionResult MyTasks()
    {
        var email = User.Identity?.Name;

        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Login", "Account");
        }

        var user = _context.Users
            .FirstOrDefault(u => u.Email == email);

        if (user == null)
        {
            return Unauthorized();
        }

        var tasks = _taskService.GetByUser(user.UserId);

        return View(tasks);
    }

    /// <summary>
    /// Displays task creation form.
    /// Managers can only assign tasks
    /// to users within their own branch.
    /// </summary>
    [Authorize(Roles = "Manager")]
    public IActionResult Create()
    {
        var branchId = int.Parse(
            User.FindFirst("BranchId")!.Value);

        // Only show users from same branch
        ViewBag.Users = _context.Users
            .Where(u => u.BranchId == branchId)
            .ToList();

        return View();
    }

    /// <summary>
    /// Handles task creation.
    /// Automatically assigns branch context.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Manager")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(TaskAssignment task)
    {
        if (!ModelState.IsValid)
        {
            var branchIdReload = int.Parse(
                User.FindFirst("BranchId")!.Value);

            ViewBag.Users = _context.Users
                .Where(u => u.BranchId == branchIdReload)
                .ToList();

            return View(task);
        }

        // Secure branch assignment
        task.BranchId = int.Parse(
            User.FindFirst("BranchId")!.Value);

        _taskService.Add(task);

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Updates task status.
    /// Staff can update only their assigned tasks.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateStatus(int id, string status)
    {
        var task = _taskService.GetById(id);

        if (task == null)
        {
            return NotFound();
        }

        // Retrieve current logged-in user
        var email = User.Identity?.Name;

        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized();
        }

        var user = _context.Users
            .FirstOrDefault(u => u.Email == email);

        if (user == null)
        {
            return Unauthorized();
        }

        // Security check:
        // Staff can only update their own tasks
        if (User.IsInRole("Staff") &&
            task.UserId != user.UserId)
        {
            return Forbid();
        }

        task.Status = status;

        _taskService.Update(task);

        return RedirectToAction(nameof(MyTasks));
    }
}