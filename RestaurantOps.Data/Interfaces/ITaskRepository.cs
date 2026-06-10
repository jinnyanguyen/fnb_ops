using RestaurantOps.Models;

namespace RestaurantOps.Data.Interfaces;

/// <summary>
/// Repository interface for task data access.
/// Responsible only for database operations.
/// </summary>
public interface ITaskRepository
{
    /// <summary>
    /// Retrieves all tasks.
    /// Used for admin/global reporting.
    /// </summary>
    List<TaskAssignment> GetAll();

    /// <summary>
    /// Retrieves tasks for a specific branch.
    /// </summary>
    List<TaskAssignment> GetAllByBranch(int branchId);

    /// <summary>
    /// Retrieves tasks assigned to a specific user.
    /// </summary>
    List<TaskAssignment> GetByUser(int userId);

    /// <summary>
    /// Retrieves a task by ID.
    /// </summary>
    TaskAssignment? GetById(int id);

    /// <summary>
    /// Adds a new task.
    /// </summary>
    void Add(TaskAssignment task);

    /// <summary>
    /// Updates an existing task.
    /// </summary>
    void Update(TaskAssignment task);

    /// <summary>
    /// Saves database changes.
    /// </summary>
    void Save();
}