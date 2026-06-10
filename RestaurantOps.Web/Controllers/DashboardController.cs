using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles dashboard display for managers.
/// Displays branch-specific operational metrics.
/// </summary>
[Authorize(Roles = "Manager")]
public class DashboardController : Controller
{
    private readonly IIngredientService _ingredientService;
    private readonly IRecipeService _recipeService;
    private readonly ITaskService _taskService;
    private readonly ISaleService _saleService;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public DashboardController(
        IIngredientService ingredientService,
        IRecipeService recipeService,
        ITaskService taskService,
        ISaleService saleService)
    {
        _ingredientService = ingredientService;
        _recipeService = recipeService;
        _taskService = taskService;
        _saleService = saleService;
    }

    /// <summary>
    /// Displays dashboard summary for the logged-in branch.
    /// </summary>
    public IActionResult Index()
    {
        // Retrieve BranchId from authentication claims
        var branchIdClaim = User.FindFirst("BranchId")?.Value;

        if (string.IsNullOrEmpty(branchIdClaim))
        {
            return Unauthorized();
        }

        int branchId = int.Parse(branchIdClaim);

        // =========================================
        // BRANCH-FILTERED DATA RETRIEVAL
        // =========================================

        var ingredients =
            _ingredientService.GetAll(branchId);

        var recipes =
            _recipeService.GetAll(branchId);

        var tasks =
            _taskService.GetAll(branchId);

        var sales =
            _saleService.GetAll(branchId);

        // =========================================
        // LOW STOCK INVENTORY ALERTS
        // =========================================

        var lowStockIngredients = ingredients
            .Where(i => i.QuantityOnHand <= i.ReorderLevel)
            .OrderBy(i => i.QuantityOnHand)
            .ToList();

        // =========================================
        // DASHBOARD KPI CALCULATIONS
        // =========================================

        // Total inventory value
        decimal totalInventoryValue = ingredients.Sum(i =>
            i.QuantityOnHand * i.CostPerUnit);

        // Total ingredient count
        int totalIngredients =
            ingredients.Count;

        // Total recipe count
        int totalRecipes =
            recipes.Count;

        // Open task count
        int openTasks = tasks.Count(t =>
            t.Status != "Completed");

        // Total sales revenue
        decimal totalSales = sales.Sum(s =>
            s.TotalAmount);

        // Low stock ingredient count
        int lowStockCount =
            lowStockIngredients.Count;

        // =========================================
        // PASS DASHBOARD DATA TO VIEW
        // =========================================

        ViewBag.TotalInventoryValue =
            totalInventoryValue;

        ViewBag.TotalIngredients =
            totalIngredients;

        ViewBag.TotalRecipes =
            totalRecipes;

        ViewBag.OpenTasks =
            openTasks;

        ViewBag.TotalSales =
            totalSales;

        ViewBag.LowStockCount =
            lowStockCount;

        ViewBag.LowStockIngredients =
            lowStockIngredients;

        // =========================================
        // SALES TREND CHART DATA
        // =========================================

        var groupedSales = sales
            .Where(s => s.SaleDate != default)
            .GroupBy(s => s.SaleDate.Date)
            .OrderBy(g => g.Key)
            .ToList();

        ViewBag.SalesDates = groupedSales
            .Select(g => g.Key.ToString("yyyy-MM-dd"))
            .ToList();

        ViewBag.SalesTotals = groupedSales
            .Select(g => g.Sum(s => s.TotalAmount))
            .ToList();

        return View();
    }
}