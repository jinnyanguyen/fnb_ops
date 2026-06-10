using Microsoft.EntityFrameworkCore;
using RestaurantOps.Data.Interfaces;
using RestaurantOps.Models;

namespace RestaurantOps.Data.Repositories;

/// <summary>
/// Handles task database operations.
/// Responsible only for data access.
/// </summary>
public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all tasks.
    /// </summary>
    public List<TaskAssignment> GetAll()
    {
        return _context.TaskAssignments
            .Include(t => t.User)
            .ToList();
    }

    /// <summary>
    /// Retrieves tasks for a specific branch.
    /// </summary>
    public List<TaskAssignment> GetAllByBranch(int branchId)
    {
        return _context.TaskAssignments
            .Include(t => t.User)
            .Where(t => t.BranchId == branchId)
            .ToList();
    }

    /// <summary>
    /// Retrieves tasks assigned to a specific user.
    /// </summary>
    public List<TaskAssignment> GetByUser(int userId)
    {
        return _context.TaskAssignments
            .Include(t => t.User)
            .Where(t => t.UserId == userId)
            .ToList();
    }

    /// <summary>
    /// Retrieves task by ID.
    /// </summary>
    public TaskAssignment? GetById(int id)
    {
        return _context.TaskAssignments
            .Include(t => t.User)
            .FirstOrDefault(t => t.TaskAssignmentId == id);
    }

    /// <summary>
    /// Adds a new task.
    /// </summary>
    public void Add(TaskAssignment task)
    {
        _context.TaskAssignments.Add(task);
    }

    /// <summary>
    /// Updates an existing task.
    /// </summary>
    public void Update(TaskAssignment task)
    {
        _context.TaskAssignments.Update(task);
    }

    /// <summary>
    /// Saves database changes.
    /// </summary>
    public void Save()
    {
        _context.SaveChanges();
    }
}