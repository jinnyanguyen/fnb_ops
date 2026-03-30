using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Defines task management operations.
/// </summary>
public interface ITaskService
{
    List<TaskAssignment> GetAll();

    List<TaskAssignment> GetByUser(int userId);

    void Add(TaskAssignment task);

    void Update(TaskAssignment task);
}