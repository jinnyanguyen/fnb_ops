using RestaurantOps.Models;


namespace RestaurantOps.Web.ViewModels;

/// <summary>
/// Represents dashboard analytics data.
/// </summary>
public class DashboardViewModel
{
    // KPI Cards
    public int TotalRecipes { get; set; }

    public int TotalIngredients { get; set; }

    public decimal TotalSales { get; set; }

    public decimal TotalInventoryValue { get; set; }

    public int OpenTasks { get; set; }

    public int LowStockCount { get; set; }

    // Low stock inventory alerts
    public List<Ingredient> LowStockIngredients
    {
        get;
        set;
    } = new();

    // Sales trend chart
    public List<string> SalesDates
    {
        get;
        set;
    } = new();

    public List<decimal> SalesTotals
    {
        get;
        set;
    } = new();

    // Staff analytics
    public List<StaffPerformanceViewModel>
        StaffPerformance
    {
        get;
        set;
    } = new();

    public List<TopSellingRecipeViewModel>
    TopSellingRecipes
    {
        get;
        set;
    } = new();

    public List<MostUsedIngredientViewModel>
    MostUsedIngredients
    { get; set; } = new();

    /// <summary>
    /// Percentage of completed tasks.
    /// </summary>
    public decimal TaskCompletionRate
    {
        get;
        set;
    }

    /// <summary>
    /// Percentage of completed SOP executions.
    /// </summary>
    public decimal SOPComplianceRate
    {
        get;
        set;
    }

    /// <summary>
    /// Active kitchen sessions.
    /// </summary>
    public int ActiveKitchenSessions
    {
        get;
        set;
    }
}