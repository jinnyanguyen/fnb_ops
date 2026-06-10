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
        var branchIdClaim = User.FindFirst("BranchId")?.Value;

        if (string.IsNullOrEmpty(branchIdClaim))
        {
            return Unauthorized();
        }

        int branchId = int.Parse(branchIdClaim);

        var sales = _saleService.GetAll(branchId);

        return View(sales);
    }


    /// <summary>
    /// Displays create sale form.
    /// </summary>
    public IActionResult Create()
    {
        var branchId = int.Parse(
            User.FindFirst("BranchId")!.Value);

        ViewBag.Recipes = _recipeService.GetAll(branchId);
        return View();
    }

    /// <summary>
    /// Handles sale creation.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Sale sale)
    {
        if (!ModelState.IsValid)
        {
            var branchIdReload = int.Parse(
                User.FindFirst("BranchId")!.Value);

            ViewBag.Recipes =
                _recipeService.GetAll(branchIdReload);

            return View(sale);
        }

        sale.BranchId = int.Parse(
            User.FindFirst("BranchId")!.Value);

        _saleService.Add(sale);

        return RedirectToAction(nameof(Index));
    }
}