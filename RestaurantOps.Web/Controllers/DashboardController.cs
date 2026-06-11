using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data;
using RestaurantOps.Models;
using RestaurantOps.Web.ViewModels;

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
        var branchIdClaim = User.FindFirst("BranchId")?.Value;

        if (string.IsNullOrEmpty(branchIdClaim))
        {
            return Unauthorized();
        }

        int branchId = int.Parse(branchIdClaim);

        var ingredients = _ingredientService.GetAll(branchId);
        var recipes = _recipeService.GetAll(branchId);
        var tasks = _taskService.GetAll(branchId);
        var sales = _saleService.GetAll(branchId);

        var sopExecutions =
            _sopExecutionService.GetExecutionsByBranch(branchId);

        var recipeExecutions =
            _recipeExecutionService.GetExecutionsByBranch(branchId);

        var staffUsers = _context.Users
            .Where(u => u.BranchId == branchId && u.Role == "Staff")
            .ToList();

        var lowStockIngredients = ingredients
            .Where(i => i.QuantityOnHand <= i.ReorderLevel)
            .OrderBy(i => i.QuantityOnHand)
            .ToList();

        var groupedSales = sales
            .Where(s => s.SaleDate != default)
            .GroupBy(s => s.SaleDate.Date)
            .OrderBy(g => g.Key)
            .ToList();

        var dashboardViewModel = new DashboardViewModel
        {
            TotalInventoryValue = ingredients.Sum(i =>
                i.QuantityOnHand * i.CostPerUnit),

            TotalIngredients = ingredients.Count,

            TotalRecipes = recipes.Count,

            OpenTasks = tasks.Count(t =>
                t.Status != "Completed"),

            TotalSales = sales.Sum(s =>
                s.TotalAmount),

            LowStockCount = lowStockIngredients.Count,

            LowStockIngredients = lowStockIngredients,

            SalesDates = groupedSales
                .Select(g => g.Key.ToString("yyyy-MM-dd"))
                .ToList(),

            SalesTotals = groupedSales
                .Select(g => g.Sum(s => s.TotalAmount))
                .ToList(),

            StaffPerformance = staffUsers
                .Select(user => new StaffPerformanceViewModel
                {
                    StaffName =
                        $"{user.FirstName} {user.LastName}",

                    SOPCompletedCount = sopExecutions.Count(e =>
                        e.UserId == user.UserId &&
                        e.ExecutionItems.Any() &&
                        e.ExecutionItems.All(i => i.IsCompleted)),

                    RecipeExecutionCount = recipeExecutions.Count(e =>
                        e.UserId == user.UserId &&
                        e.ExecutionSteps.Any() &&
                        e.ExecutionSteps.All(s => s.IsCompleted)),

                    TaskCompletedCount = tasks.Count(t =>
                        t.UserId == user.UserId &&
                        t.Status == "Completed"),

                    OverdueTaskCount = tasks.Count(t =>
                        t.UserId == user.UserId &&
                        t.IsOverdue)
                })
                .ToList()
        };

        return View(dashboardViewModel);
    }
}