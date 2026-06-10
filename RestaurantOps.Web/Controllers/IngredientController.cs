using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Models;
using RestaurantOps.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using RestaurantOps.Business.Helpers;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles all Ingredient-related operations (CRUD).
/// </summary>
[Authorize]
public class IngredientController : Controller
{
    private readonly IIngredientService _service;

    /// <summary>
    /// Constructor with dependency injection
    /// </summary>
    public IngredientController(IIngredientService service)
    {
        _service = service;
    }

    /// <summary>
    /// Displays all ingredients
    /// </summary>
    public IActionResult Index()
{
    int branchId = BranchHelper.GetBranchId(User);

    var ingredients = _service.GetAll(branchId);

   ViewBag.TotalValue =
    _service.GetTotalInventoryValue(branchId);

    return View(ingredients);
}

    /// <summary>
    /// Shows create form
    /// </summary>
    [Authorize(Roles = "Manager")]
    public IActionResult Create()
    {
        return View();
    }

   /// <summary>
/// Handles ingredient creation.
/// Automatically assigns ingredient to logged-in branch.
/// </summary>
[Authorize(Roles = "Manager")]
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(Ingredient ingredient)
{
    try
    {
        if (ModelState.IsValid)
        {
            // Retrieve BranchId from logged-in user
            int branchId =
                BranchHelper.GetBranchId(User);

            // Assign ingredient to branch
            ingredient.BranchId = branchId;

            _service.Add(ingredient);

            return RedirectToAction(nameof(Index));
        }
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", ex.Message);
    }

    return View(ingredient);
}

    /// <summary>
    /// Displays edit form
    /// </summary>
    [Authorize(Roles = "Manager")]
    public IActionResult Edit(int id)
    {
        var ingredient = _service.GetById(id);

        if (ingredient == null)
        {
            return NotFound();
        }

        return View(ingredient);
    }

    /// <summary>
    /// Handles edit form submission
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Manager")]
    public IActionResult Edit(Ingredient ingredient)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _service.Update(ingredient);
                return RedirectToAction("Index");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        return View(ingredient);
    }

    /// <summary>
    /// Shows delete confirmation page
    /// </summary>
    [Authorize(Roles = "Manager")]
    public IActionResult Delete(int id)
    {
        var ingredient = _service.GetById(id);

        if (ingredient == null)
        {
            return NotFound();
        }

        return View(ingredient);
    }

    /// <summary>
    /// Confirms delete action
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Manager")]
    public IActionResult DeleteConfirmed(int id)
    {
        _service.Delete(id);
        return RedirectToAction("Index");
    }
}