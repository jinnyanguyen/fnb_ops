using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Models;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles sales pages and actions.
/// Only managers can access sales management.
/// </summary>
[Authorize(Roles = "Manager")]
public class SaleController : Controller
{
    private readonly ISaleService _saleService;
    private readonly IRecipeService _recipeService;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public SaleController(ISaleService saleService, IRecipeService recipeService)
    {
        _saleService = saleService;
        _recipeService = recipeService;
    }

    /// <summary>
    /// Displays all sales records.
    /// </summary>
    public IActionResult Index()
    {
        var sales = _saleService.GetAll();
        return View(sales);
    }

    /// <summary>
    /// Displays create sale form.
    /// </summary>
    public IActionResult Create()
    {
        ViewBag.Recipes = _recipeService.GetAll();
        return View();
    }

    /// <summary>
    /// Handles sale creation.
    /// </summary>
    [HttpPost]
    public IActionResult Create(Sale sale)
    {
        if (ModelState.IsValid)
        {
            _saleService.Add(sale);
            return RedirectToAction("Index");
        }

        ViewBag.Recipes = _recipeService.GetAll();
        return View(sale);
    }
}