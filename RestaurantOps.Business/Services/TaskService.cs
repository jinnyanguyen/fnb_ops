using Microsoft.Extensions.Logging;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data.Interfaces;
using RestaurantOps.Models;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Handles task-related business logic.
/// Responsible for:
/// - Task assignment
/// - Task updates
/// - Branch-aware task filtering
/// 
/// Database access is delegated to repositories.
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly ILogger<TaskService> _logger;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public TaskService(
        ITaskRepository repository,
        ILogger<TaskService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all tasks.
    /// Used for admin/global reporting.
    /// </summary>
    public List<TaskAssignment> GetAll()
    {
        _logger.LogInformation("Fetching all tasks");

        return _repository.GetAll();
    }

    /// <summary>
    /// Retrieves tasks for a specific branch.
    /// </summary>
    /// <param name="branchId">
    /// Branch identifier.
    /// </param>
    public List<TaskAssignment> GetAll(int branchId)
    {
        _logger.LogInformation(
            "Fetching tasks for branch ID: {BranchId}",
            branchId);

        return _repository.GetAllByBranch(branchId);
    }

    /// <summary>
    /// Retrieves tasks assigned to a specific user.
    /// </summary>
    /// <param name="userId">
    /// User identifier.
    /// </param>
    public List<TaskAssignment> GetByUser(int userId)
    {
        _logger.LogInformation(
            "Fetching tasks for user ID: {UserId}",
            userId);

        return _repository.GetByUser(userId);
    }

    /// <summary>
    /// Retrieves task by ID.
    /// </summary>
    /// <param name="id">
    /// Task identifier.
    /// </param>
    public TaskAssignment? GetById(int id)
    {
        _logger.LogInformation(
            "Fetching task ID: {TaskId}",
            id);

        return _repository.GetById(id);
    }

    /// <summary>
    /// Adds a new task.
    /// </summary>
    /// <param name="task">
    /// Task to create.
    /// </param>
    public void Add(TaskAssignment task)
    {
        _logger.LogInformation(
            "Creating task: {Title}",
            task.Title);

        _repository.Add(task);
        _repository.Save();
    }

    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="task">
    /// Updated task object.
    /// </param>
    public void Update(TaskAssignment task)
    {
        _logger.LogInformation(
            "Updating task ID: {TaskId}",
            task.TaskAssignmentId);

        _repository.Update(task);
        _repository.Save();
    }
}