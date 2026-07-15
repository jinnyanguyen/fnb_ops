using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Models;
using RestaurantOps.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using RestaurantOps.Business.Helpers;
using RestaurantOps.Web.ViewModels;
using RestaurantOps.Data.Interfaces;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles all Ingredient-related operations (CRUD).
/// </summary>
[Authorize]
public class IngredientController : Controller
{
    private readonly IIngredientService _service;
    private readonly
    IInventoryTransactionRepository
    _inventoryTransactionRepository;

    /// <summary>
    /// Constructor with dependency injection
    /// </summary>
    public IngredientController(
    IIngredientService service,
    IInventoryTransactionRepository inventoryTransactionRepository)
    {
        _service = service;
        _inventoryTransactionRepository =
            inventoryTransactionRepository;
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
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Ingredient ingredient)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(ingredient);
            }

            var existingIngredient =
                _service.GetById(ingredient.IngredientId);

            if (existingIngredient == null)
            {
                return NotFound();
            }

            existingIngredient.Name =
                ingredient.Name;

            existingIngredient.Unit =
                ingredient.Unit;

            existingIngredient.QuantityOnHand =
                ingredient.QuantityOnHand;

            existingIngredient.CostPerUnit =
                ingredient.CostPerUnit;

            existingIngredient.ReorderLevel =
                ingredient.ReorderLevel;

            // IMPORTANT:
            // DO NOT overwrite BranchId

            _service.Update(existingIngredient);

            return RedirectToAction(nameof(Index));
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

    /// <summary>
    /// Displays stock adjustment page.
    /// </summary>
    [Authorize(Roles = "Manager")]
    public IActionResult AdjustStock(int id)
    {
        var ingredient =
            _service.GetById(id);

        if (ingredient == null)
        {
            return NotFound();
        }

        var model =
            new InventoryAdjustmentViewModel
            {
                IngredientId =
                    ingredient.IngredientId,

                IngredientName =
                    ingredient.Name
            };

        return View(model);
    }

    /// <summary>
    /// Handles inventory stock refill.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Manager")]
    public IActionResult AdjustStock(
        InventoryAdjustmentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var ingredient =
            _service.GetById(
                model.IngredientId);

        if (ingredient == null)
        {
            return NotFound();
        }

        ingredient.QuantityOnHand +=
            model.QuantityToAdd;

        _service.Update(
            ingredient);

        var branchId =
            BranchHelper.GetBranchId(
                User);

        _inventoryTransactionRepository.Add(
            new InventoryTransaction
            {
                IngredientId =
                    ingredient.IngredientId,

                QuantityChanged =
                    model.QuantityToAdd,

                Reason =
                    model.Reason,

                TransactionDate =
                    DateTime.Now,

                BranchId =
                    branchId
            });

        _inventoryTransactionRepository.Save();

        return RedirectToAction(
            nameof(Index));
    }
}