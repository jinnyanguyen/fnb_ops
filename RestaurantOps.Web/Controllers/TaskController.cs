using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Models;
using RestaurantOps.Data;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles task assignment and updates.
/// </summary>
[Authorize]
public class TaskController : Controller
{
    private readonly ITaskService _taskService;
    private readonly ApplicationDbContext _context;

    public TaskController(ITaskService taskService, ApplicationDbContext context)
    {
        _taskService = taskService;
        _context = context;
    }

    /// <summary>
    /// Manager view: all tasks
    /// </summary>
    public IActionResult Index()
    {
        if (User.IsInRole("Manager"))
        {
            var tasks = _taskService.GetAll();
            return View(tasks);
        }

        // If not manager → redirect to MyTasks
        return RedirectToAction("MyTasks");
    }

    /// <summary>
    /// Staff view: only their tasks
    /// </summary>
    public IActionResult MyTasks()
    {
        var email = User.Identity?.Name;

        if (string.IsNullOrEmpty(email))
            return RedirectToAction("Login", "Account");

        var user = _context.Users.FirstOrDefault(u => u.Email == email);

        if (user == null)
            return Unauthorized();

        var tasks = _taskService.GetByUser(user.UserId);

        return View(tasks);
    }

    /// <summary>
    /// Show create task form
    /// </summary>
    [Authorize(Roles = "Manager")]
    public IActionResult Create()
    {
        ViewBag.Users = _context.Users.ToList();
        return View();
    }

    /// <summary>
    /// Handle task creation
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Manager")]
    public IActionResult Create(TaskAssignment task)
    {
        if (ModelState.IsValid)
        {
            _taskService.Add(task);
            return RedirectToAction("Index");
        }

        ViewBag.Users = _context.Users.ToList();
        return View(task);
    }

    /// <summary>
    /// Staff updates task status
    /// </summary>
    [HttpPost]
    public IActionResult UpdateStatus(int id, string status)
    {
        var task = _context.TaskAssignments.Find(id);

        if (task == null)
            return NotFound();

        task.Status = status;
        _taskService.Update(task);

        return RedirectToAction("MyTasks");
    }
}