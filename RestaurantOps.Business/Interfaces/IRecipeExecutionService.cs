using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Handles recipe execution workflows.
/// </summary>
public interface IRecipeExecutionService
{
    /// <summary>
    /// Starts a recipe execution session.
    /// </summary>
    RecipeExecution StartExecution(
        int recipeId,
        int userId,
        int branchId);

    /// <summary>
    /// Retrieves recipe execution session.
    /// </summary>
    RecipeExecution? GetExecution(int executionId);

    /// <summary>
    /// Completes a recipe execution step.
    /// </summary>
    void CompleteStep(int executionStepId);

    /// <summary>
    /// Retrieves recipe executions for branch monitoring.
    /// </summary>
    List<RecipeExecution> GetExecutionsByBranch(int branchId);

    /// <summary>
    /// Retrieves recipe executions for a user.
    /// </summary>
    List<RecipeExecution> GetExecutionsByUser(int userId);
}