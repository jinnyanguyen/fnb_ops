using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Models;
using RestaurantOps.Business.Interfaces;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles all Ingredient-related operations (CRUD).
/// </summary>
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
        var ingredients = _service.GetAll();
        return View(ingredients);
    }

    /// <summary>
    /// Shows create form
    /// </summary>
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Handles form submission
    /// </summary>
    [HttpPost]
    [HttpPost]
    public IActionResult Create(Ingredient ingredient)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _service.Add(ingredient);
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
    /// Displays edit form
    /// </summary>
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
    public IActionResult DeleteConfirmed(int id)
    {
        _service.Delete(id);
        return RedirectToAction("Index");
    }
}