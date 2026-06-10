using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Handles SOP execution workflow operations.
/// </summary>
public interface ISOPExecutionService
{
    /// <summary>
    /// Starts an SOP execution session.
    /// </summary>
    SOPExecution StartExecution(
        int sopTemplateId,
        int userId,
        int branchId);

    /// <summary>
    /// Retrieves execution by ID.
    /// </summary>
    SOPExecution? GetExecution(int executionId);

    /// <summary>
    /// Marks checklist item as completed.
    /// </summary>
    void CompleteItem(int executionItemId);

    /// <summary>
    /// Retrieves SOP executions for a branch.
    /// </summary>
    List<SOPExecution> GetExecutionsByBranch(int branchId);

    /// <summary>
    /// Retrieves SOP executions for a user.
    /// </summary>
    List<SOPExecution> GetExecutionsByUser(int userId);
}
