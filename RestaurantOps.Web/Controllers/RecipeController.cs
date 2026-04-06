using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Business.Services;
using RestaurantOps.Models;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles Recipe-related operations (CRUD + cost display).
/// </summary>
[Authorize(Roles = "Manager")]
public class RecipeController : Controller
{
    private readonly IRecipeService _recipeService;
    private readonly IIngredientService _ingredientService;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public RecipeController(IRecipeService recipeService, IIngredientService ingredientService)
    {
        _recipeService = recipeService;
        _ingredientService = ingredientService;
    }

    /// <summary>
    /// Displays all recipes.
    /// </summary>
    public IActionResult Index()
    {
        var recipes = _recipeService.GetAll();
        return View(recipes);
    }

    /// <summary>
    /// Displays create recipe form.
    /// </summary>
    public IActionResult Create()
    {
        ViewBag.Ingredients = _ingredientService.GetAll();
        return View();
    }

    /// <summary>
    /// Handles recipe creation.
    /// </summary>
    [HttpPost]
    public IActionResult Create(Recipe recipe)
    {
        if (ModelState.IsValid)
        {
            _recipeService.Add(recipe);
            return RedirectToAction("Index");
        }

        // Re-populate dropdown if validation fails
        ViewBag.Ingredients = _ingredientService.GetAll();

        return View(recipe);
    }

    /// <summary>
/// Displays recipe details along with cost and profit analysis.
/// </summary>
public IActionResult Details(int id)
{
    var recipe = _recipeService.GetById(id);

    if (recipe == null)
        return NotFound();
    var cost = _recipeService.CalculateRecipeCost(id);
    var profit = _recipeService.CalculateProfit(id);
    var margin = _recipeService.CalculateProfitMargin(id);

    Console.WriteLine($"Debug cost: {cost}");
    Console.WriteLine($"Debug profit: {profit}");
    Console.WriteLine($"Debug margin: {margin}");

    ViewBag.Cost = cost;
    ViewBag.Profit = profit;
    ViewBag.Margin = margin;

    ViewBag.Ingredients = _ingredientService.GetAll();
    return View(recipe);
}

    /// <summary>
    /// Adds ingredient to recipe.
    /// </summary>
    [HttpPost]
    public IActionResult AddIngredient(int recipeId, int ingredientId, decimal quantity)
    {
        _recipeService.AddIngredientToRecipe(recipeId, ingredientId, quantity);
        return RedirectToAction("Details", new { id = recipeId });
    }

    /// <summary>
    /// Removes an ingredient from a recipe.
    /// </summary>
    [HttpPost]
    public IActionResult RemoveIngredient(int recipeId, int ingredientId)
    {
        _recipeService.RemoveIngredientFromRecipe(recipeId, ingredientId);
        return RedirectToAction("Details", new { id = recipeId });
    }

    /// <summary>
    /// Edit an ingredient from a recipe, get edit
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IActionResult Edit(int id)
    {
        var recipe = _recipeService.GetById(id);

        if (recipe == null)
            return NotFound();

        return View(recipe);
    }

    /// <summary>
    /// Handles edit ingredient from recipe
    /// </summary>
    /// <param name="recipe"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Edit(Recipe recipe)
    {
        if (ModelState.IsValid)
        {
            _recipeService.Update(recipe);
            return RedirectToAction("Index");
        }

        return View(recipe);
    }
}