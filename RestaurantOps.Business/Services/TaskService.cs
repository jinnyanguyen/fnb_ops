using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data;
using RestaurantOps.Models;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Handles business logic for task management.
/// </summary>
public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ApplicationDbContext context, ILogger<TaskService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all tasks
    /// </summary>
    public List<TaskAssignment> GetAll()
    {
        return _context.TaskAssignments
            .Include(t => t.User)
            .ToList();
    }

    /// <summary>
    /// Get tasks for a specific user
    /// </summary>
    public List<TaskAssignment> GetByUser(int userId)
    {
        return _context.TaskAssignments
            .Where(t => t.UserId == userId)
            .Include(t => t.User)
            .ToList();
    }

    /// <summary>
    /// Add new task
    /// </summary>
    public void Add(TaskAssignment task)
    {
        _logger.LogInformation("Creating task: {Title}", task.Title);

        _context.TaskAssignments.Add(task);
        _context.SaveChanges();
    }

    /// <summary>
    /// Update task status
    /// </summary>
    public void Update(TaskAssignment task)
    {
        _logger.LogInformation("Updating task ID: {Id}", task.TaskAssignmentId);

        _context.TaskAssignments.Update(task);
        _context.SaveChanges();
    }
}