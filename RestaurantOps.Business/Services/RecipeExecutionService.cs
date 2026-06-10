using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data;
using RestaurantOps.Models;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Handles live kitchen recipe execution workflows.
/// </summary>
public class RecipeExecutionService
    : IRecipeExecutionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RecipeExecutionService> _logger;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public RecipeExecutionService(
        ApplicationDbContext context,
        ILogger<RecipeExecutionService> logger)
    {
        _context = context;
        _logger = logger;
    }

   /// <summary>
/// Starts recipe execution session.
/// </summary>
public RecipeExecution StartExecution(
    int recipeId,
    int userId,
    int branchId)
{
    // Load recipe and recipe steps
    var recipe = _context.Recipes
        .Include(r => r.RecipeSteps)
        .FirstOrDefault(r => r.RecipeId == recipeId);

    if (recipe == null)
    {
        throw new Exception("Recipe not found.");
    }

    // Create execution session
    var execution = new RecipeExecution
    {
        RecipeId = recipeId,
        UserId = userId,
        BranchId = branchId,
        StartedAt = DateTime.Now,

        // Initialize collection
        ExecutionSteps = new List<RecipeExecutionStep>()
    };

    // Create execution steps
    foreach (var step in recipe.RecipeSteps)
    {
        execution.ExecutionSteps.Add(
            new RecipeExecutionStep
            {
                RecipeStepId = step.RecipeStepId,
                IsCompleted = false
            });
    }

    _context.RecipeExecutions.Add(execution);

    _context.SaveChanges();

    _logger.LogInformation(
        "Recipe execution started for recipe ID: {RecipeId}",
        recipeId);

    return execution;
}

    /// <summary>
    /// Retrieves recipe execution session.
    /// </summary>
    public RecipeExecution? GetExecution(int executionId)
    {
        return _context.RecipeExecutions
            .Include(e => e.Recipe)
            .Include(e => e.ExecutionSteps)
                .ThenInclude(s => s.RecipeStep)
            .Include(e => e.User)
            .FirstOrDefault(e =>
                e.RecipeExecutionId == executionId);
    }

    /// <summary>
    /// Completes recipe execution step.
    /// </summary>
    public void CompleteStep(int executionStepId)
    {
        var step = _context.RecipeExecutionSteps
            .FirstOrDefault(s =>
                s.RecipeExecutionStepId == executionStepId);

        if (step == null)
        {
            throw new Exception(
                "Execution step not found.");
        }

        step.IsCompleted = true;
        step.CompletedAt = DateTime.Now;

        _context.SaveChanges();

        _logger.LogInformation(
            "Recipe execution step completed: {StepId}",
            executionStepId);
    }

    /// <summary>
    /// Retrieves recipe executions for branch monitoring.
    /// Includes recipe, staff user, execution steps, and original recipe step details.
    /// </summary>
    public List<RecipeExecution> GetExecutionsByBranch(int branchId)
    {
        return _context.RecipeExecutions
            .Include(e => e.Recipe)
            .Include(e => e.User)
            .Include(e => e.ExecutionSteps)
                .ThenInclude(es => es.RecipeStep)
            .Where(e => e.BranchId == branchId)
            .OrderByDescending(e => e.StartedAt)
            .ToList();
    }

    /// <summary>
    /// Retrieves recipe executions for a specific staff user.
    /// Includes recipe and execution step details for history display.
    /// </summary>
    public List<RecipeExecution> GetExecutionsByUser(int userId)
    {
        return _context.RecipeExecutions
            .Include(e => e.Recipe)
            .Include(e => e.ExecutionSteps)
                .ThenInclude(es => es.RecipeStep)
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.StartedAt)
            .ToList();
    }
}