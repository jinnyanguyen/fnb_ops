using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Helpers;
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
    /// Displays recipes for the logged-in user's branch.
    /// </summary>
    public IActionResult Index()
    {
        var branchIdClaim = User.FindFirst("BranchId")?.Value;

        if (string.IsNullOrEmpty(branchIdClaim))
        {
            return Unauthorized();
        }

        int branchId = int.Parse(branchIdClaim);

        var recipes = _recipeService.GetAll(branchId);

        return View(recipes);
    }

    /// <summary>
    /// Adds a preparation step to a recipe.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddStep(RecipeStep step)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(
                nameof(Details),
                new { id = step.RecipeId });
        }

        _recipeService.AddStep(step);

        return RedirectToAction(
            nameof(Details),
            new { id = step.RecipeId });
    }

    /// <summary>
    /// Displays create recipe form.
    /// </summary>
    public IActionResult Create()
    {
        int branchId = BranchHelper.GetBranchId(User);

        ViewBag.Ingredients = _ingredientService.GetAll(branchId); return View();
    }

    /// <summary>
    /// Handles recipe creation.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Recipe recipe)
    {
        if (!ModelState.IsValid)
            return View(recipe);

        recipe.BranchId = int.Parse(
            User.FindFirst("BranchId")!.Value);

        _recipeService.Add(recipe);

        return RedirectToAction(nameof(Index));
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

        int branchId = BranchHelper.GetBranchId(User);

        ViewBag.Ingredients = _ingredientService.GetAll(branchId); return View(recipe);
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