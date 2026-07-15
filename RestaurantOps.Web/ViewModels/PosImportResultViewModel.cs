namespace RestaurantOps.Web.ViewModels;

/// <summary>
/// Represents the result of importing POS sales data.
/// </summary>
public class PosImportResultViewModel
{
    public int TotalRows { get; set; }

    public int SuccessfulImports { get; set; }

    public int FailedImports { get; set; }

    public List<string> Errors { get; set; } = new();
}