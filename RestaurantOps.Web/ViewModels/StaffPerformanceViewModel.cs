namespace RestaurantOps.Web.ViewModels;

/// <summary>
/// Represents staff operational performance metrics.
/// </summary>
public class StaffPerformanceViewModel
{
    /// <summary>
    /// Staff member name.
    /// </summary>
    public string StaffName { get; set; } = string.Empty;

    /// <summary>
    /// Total SOP executions completed.
    /// </summary>
    public int SOPCompletedCount { get; set; }

    /// <summary>
    /// Total kitchen recipe executions completed.
    /// </summary>
    public int RecipeExecutionCount { get; set; }

    /// <summary>
    /// Total completed tasks.
    /// </summary>
    public int TaskCompletedCount { get; set; }

    /// <summary>
    /// Total overdue tasks.
    /// </summary>
    public int OverdueTaskCount { get; set; }
}