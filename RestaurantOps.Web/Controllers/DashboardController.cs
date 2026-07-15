using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data;
using RestaurantOps.Models;
using RestaurantOps.Web.ViewModels;
using System.Text;

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
    private readonly ISOPExecutionService _sopExecutionService;
    private readonly IRecipeExecutionService _recipeExecutionService;
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public DashboardController(
        IIngredientService ingredientService,
        IRecipeService recipeService,
        ITaskService taskService,
        ISaleService saleService,
        ISOPExecutionService sopExecutionService,
        IRecipeExecutionService recipeExecutionService,
        ApplicationDbContext context)
    {
        _ingredientService = ingredientService;
        _recipeService = recipeService;
        _taskService = taskService;
        _saleService = saleService;
        _sopExecutionService = sopExecutionService;
        _recipeExecutionService = recipeExecutionService;
        _context = context;
    }

    /// <summary>
/// Displays dashboard summary for the logged-in branch.
/// </summary>
public IActionResult Index()
{
    var branchIdClaim =
        User.FindFirst("BranchId")?.Value;

    if (string.IsNullOrEmpty(branchIdClaim))
    {
        return Unauthorized();
    }

    int branchId =
        int.Parse(branchIdClaim);

    // =========================================
    // BRANCH DATA
    // =========================================

    var ingredients =
        _ingredientService.GetAll(branchId);

    var recipes =
        _recipeService.GetAll(branchId);

    var tasks =
        _taskService.GetAll(branchId);

    var sales =
        _saleService.GetAll(branchId);

    var sopExecutions =
        _sopExecutionService
            .GetExecutionsByBranch(branchId);

    var recipeExecutions =
        _recipeExecutionService
            .GetExecutionsByBranch(branchId);

    var staffUsers =
        _context.Users
            .Where(u =>
                u.BranchId == branchId
                &&
                u.Role == "Staff")
            .ToList();

    // =========================================
    // LOW STOCK ALERTS
    // =========================================

    var lowStockIngredients =
        ingredients
            .Where(i =>
                i.QuantityOnHand <= i.ReorderLevel)
            .OrderBy(i =>
                i.QuantityOnHand)
            .ToList();

    // =========================================
    // SALES TREND
    // =========================================

    var groupedSales =
        sales
            .Where(s =>
                s.SaleDate != default)
            .GroupBy(s =>
                s.SaleDate.Date)
            .OrderBy(g =>
                g.Key)
            .ToList();

    // =========================================
    // TOP SELLING RECIPES
    // =========================================

    var topSellingRecipes =
        sales
            .GroupBy(s =>
                s.Recipe?.Name)
            .Select(g =>
                new TopSellingRecipeViewModel
                {
                    RecipeName =
                        g.Key ?? "Unknown",

                    QuantitySold =
                        g.Sum(x =>
                            x.QuantitySold)
                })
            .OrderByDescending(x =>
                x.QuantitySold)
            .Take(5)
            .ToList();

    // =========================================
    // MOST USED INGREDIENTS
    // =========================================

    var mostUsedIngredients =
        _context.InventoryTransactions
            .Where(t =>
                t.BranchId == branchId
                &&
                t.QuantityChanged < 0)
            .GroupBy(t =>
                t.Ingredient!.Name)
            .Select(g =>
                new MostUsedIngredientViewModel
                {
                    IngredientName =
                        g.Key,

                    QuantityUsed =
                        Math.Abs(
                            g.Sum(x =>
                                x.QuantityChanged))
                })
            .OrderByDescending(x =>
                x.QuantityUsed)
            .Take(5)
            .ToList();

    // =========================================
    // TASK COMPLETION RATE
    // =========================================

    decimal taskCompletionRate = 0;

    if (tasks.Any())
    {
        taskCompletionRate =
            (decimal)tasks.Count(t =>
                t.Status == "Completed")
            /
            tasks.Count
            * 100;
    }

    // =========================================
    // SOP COMPLIANCE RATE
    // =========================================

    decimal sopComplianceRate = 0;

    if (sopExecutions.Any())
    {
        sopComplianceRate =
            (decimal)sopExecutions.Count(e =>
                e.ExecutionItems.Any()
                &&
                e.ExecutionItems.All(i =>
                    i.IsCompleted))
            /
            sopExecutions.Count
            * 100;
    }

    // =========================================
    // ACTIVE KITCHEN SESSIONS
    // =========================================

    int activeKitchenSessions =
        recipeExecutions.Count(e =>
            e.ExecutionSteps.Any()
            &&
            !e.ExecutionSteps.All(s =>
                s.IsCompleted));

    // =========================================
    // BUILD VIEW MODEL
    // =========================================

    var dashboardViewModel =
        new DashboardViewModel
        {
            TotalInventoryValue =
                ingredients.Sum(i =>
                    i.QuantityOnHand *
                    i.CostPerUnit),

            TotalIngredients =
                ingredients.Count,

            TotalRecipes =
                recipes.Count,

            OpenTasks =
                tasks.Count(t =>
                    t.Status != "Completed"),

            TotalSales =
                sales.Sum(s =>
                    s.TotalAmount),

            LowStockCount =
                lowStockIngredients.Count,

            LowStockIngredients =
                lowStockIngredients,

            SalesDates =
                groupedSales
                    .Select(g =>
                        g.Key.ToString("yyyy-MM-dd"))
                    .ToList(),

            SalesTotals =
                groupedSales
                    .Select(g =>
                        g.Sum(s =>
                            s.TotalAmount))
                    .ToList(),

            TopSellingRecipes =
                topSellingRecipes,

            MostUsedIngredients =
                mostUsedIngredients,

            TaskCompletionRate =
                taskCompletionRate,

            SOPComplianceRate =
                sopComplianceRate,

            ActiveKitchenSessions =
                activeKitchenSessions,

            StaffPerformance =
                staffUsers
                    .Select(user =>
                        new StaffPerformanceViewModel
                        {
                            StaffName =
                                $"{user.FirstName} {user.LastName}",

                            SOPCompletedCount =
                                sopExecutions.Count(e =>
                                    e.UserId == user.UserId
                                    &&
                                    e.ExecutionItems.Any()
                                    &&
                                    e.ExecutionItems.All(i =>
                                        i.IsCompleted)),

                            RecipeExecutionCount =
                                recipeExecutions.Count(e =>
                                    e.UserId == user.UserId
                                    &&
                                    e.ExecutionSteps.Any()
                                    &&
                                    e.ExecutionSteps.All(s =>
                                        s.IsCompleted)),

                            TaskCompletedCount =
                                tasks.Count(t =>
                                    t.UserId == user.UserId
                                    &&
                                    t.Status == "Completed"),

                            OverdueTaskCount =
                                tasks.Count(t =>
                                    t.UserId == user.UserId
                                    &&
                                    t.IsOverdue)
                        })
                    .ToList()
        };

    return View(dashboardViewModel);
}

    /// <summary>
    /// Exports inventory report to CSV.
    /// </summary>
    [Authorize(Roles = "Manager")]
    public IActionResult ExportInventory()
    {
        var branchId =
            int.Parse(
                User.FindFirst("BranchId")!.Value);

        var ingredients =
            _ingredientService.GetAll(branchId);

        var csv =
            new StringBuilder();

        csv.AppendLine(
            "Ingredient,Quantity,Unit,Cost Per Unit,Inventory Value");

        foreach (var ingredient in ingredients)
        {
            csv.AppendLine(
                $"{ingredient.Name}," +
                $"{ingredient.QuantityOnHand}," +
                $"{ingredient.Unit}," +
                $"{ingredient.CostPerUnit}," +
                $"{ingredient.QuantityOnHand * ingredient.CostPerUnit}");
        }

        return File(
            Encoding.UTF8.GetBytes(csv.ToString()),
            "text/csv",
            "InventoryReport.csv");
    }

    /// <summary>
    /// Exports sales report to CSV.
    /// </summary>
    [Authorize(Roles = "Manager")]
    public IActionResult ExportSales()
    {
        var branchId =
            int.Parse(
                User.FindFirst("BranchId")!.Value);

        var sales =
            _saleService.GetAll(branchId);

        var csv =
            new StringBuilder();

        csv.AppendLine(
            "Date,Recipe,Quantity Sold,Total Amount");

        foreach (var sale in sales)
        {
            csv.AppendLine(
                $"{sale.SaleDate:yyyy-MM-dd}," +
                $"{sale.Recipe?.Name}," +
                $"{sale.QuantitySold}," +
                $"{sale.TotalAmount}");
        }

        return File(
            Encoding.UTF8.GetBytes(csv.ToString()),
            "text/csv",
            "SalesReport.csv");
    }

    /// <summary>
/// Exports staff performance report.
/// </summary>
[Authorize(Roles = "Manager")]
public IActionResult ExportStaffPerformance()
{
    var branchId =
        int.Parse(
            User.FindFirst("BranchId")!.Value);

    var staffUsers =
        _context.Users
            .Where(u =>
                u.BranchId == branchId &&
                u.Role == "Staff")
            .ToList();

    var tasks =
        _taskService.GetAll(branchId);

    var sopExecutions =
        _sopExecutionService
            .GetExecutionsByBranch(branchId);

    var recipeExecutions =
        _recipeExecutionService
            .GetExecutionsByBranch(branchId);

    var csv =
        new StringBuilder();

    csv.AppendLine(
        "Staff Name,Tasks Completed,SOPs Completed,Recipes Executed,Overdue Tasks");

    foreach (var user in staffUsers)
    {
        var taskCompletedCount =
            tasks.Count(t =>
                t.UserId == user.UserId &&
                t.Status == "Completed");

        var overdueTaskCount =
            tasks.Count(t =>
                t.UserId == user.UserId &&
                t.IsOverdue);

        var sopCompletedCount =
            sopExecutions.Count(e =>
                e.UserId == user.UserId);

        var recipeExecutionCount =
            recipeExecutions.Count(e =>
                e.UserId == user.UserId);

        csv.AppendLine(
            $"{user.FirstName} {user.LastName}," +
            $"{taskCompletedCount}," +
            $"{sopCompletedCount}," +
            $"{recipeExecutionCount}," +
            $"{overdueTaskCount}");
    }

    return File(
        Encoding.UTF8.GetBytes(csv.ToString()),
        "text/csv",
        "StaffPerformanceReport.csv");
}
}