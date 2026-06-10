using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Defines task management business operations.
/// </summary>
public interface ITaskService
{
    /// <summary>
    /// Retrieves all tasks.
    /// </summary>
    List<TaskAssignment> GetAll();

    /// <summary>
    /// Retrieves tasks for a specific branch.
    /// </summary>
    List<TaskAssignment> GetAll(int branchId);

    /// <summary>
    /// Retrieves tasks assigned to a user.
    /// </summary>
    List<TaskAssignment> GetByUser(int userId);

    /// <summary>
    /// Retrieves task by ID.
    /// </summary>
    TaskAssignment? GetById(int id);

    /// <summary>
    /// Adds a new task.
    /// </summary>
    void Add(TaskAssignment task);

    /// <summary>
    /// Updates task.
    /// </summary>
    void Update(TaskAssignment task);
}