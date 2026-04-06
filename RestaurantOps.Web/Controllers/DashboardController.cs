using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Business.Services;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles dashboard display for managers.
/// </summary>
[Authorize(Roles = "Manager")]
public class DashboardController : Controller
{
    private readonly IIngredientService _ingredientService;
    private readonly IRecipeService _recipeService;
    private readonly ITaskService _taskService;
    private readonly ISaleService _saleService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ingredientService"></param>
    /// <param name="recipeService"></param>
    /// <param name="taskService"></param>
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
    /// Displays dashboard summary.
    /// </summary>
    public IActionResult Index()
    {
        var ingredients = _ingredientService.GetAll();
        var recipes = _recipeService.GetAll();
        var tasks = _taskService.GetAll();
        var sales = _saleService.GetAll();

        // Total Inventory Value
        decimal totalInventoryValue = ingredients.Sum(i => i.QuantityOnHand * i.CostPerUnit);

        // Total Ingredients
        int totalIngredients = ingredients.Count;

        // Total Recipes
        int totalRecipes = recipes.Count;

        // Open Tasks
        int openTasks = tasks.Count(t => t.Status != "Completed");

        // Total Sales Revenue
        decimal totalSales = sales.Sum(s => s.TotalAmount);

        ViewBag.TotalInventoryValue = totalInventoryValue;
        ViewBag.TotalIngredients = totalIngredients;
        ViewBag.TotalRecipes = totalRecipes;
        ViewBag.OpenTasks = openTasks;
        ViewBag.TotalSales = totalSales;

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