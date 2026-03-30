using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles dashboard display for managers.
/// </summary>
[Authorize(Roles = "Manager")]
public class DashboardController : Controller
{
    private readonly IIngredientService _ingredientService;

    public DashboardController(IIngredientService ingredientService)
    {
        _ingredientService = ingredientService;
    }

    /// <summary>
    /// Displays dashboard summary.
    /// </summary>
    public IActionResult Index()
    {
        var ingredients = _ingredientService.GetAll();

        var totalValue = _ingredientService.GetTotalInventoryValue();

        var totalItems = ingredients.Count;

        // Simple low stock logic (example: quantity < 5)
        var lowStockCount = ingredients.Count(i => i.QuantityOnHand < 5);

        ViewBag.TotalValue = totalValue;
        ViewBag.TotalItems = totalItems;
        ViewBag.LowStock = lowStockCount;

        return View();
    }
}