using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data;
using RestaurantOps.Models;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Handles all business logic related to Recipes.
/// This includes retrieving recipes, managing recipe ingredients,
/// and calculating total recipe cost.
/// </summary>
public class RecipeService : IRecipeService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RecipeService> _logger;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="logger">Logger for tracking operations</param>
    public RecipeService(ApplicationDbContext context, ILogger<RecipeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all recipes including their ingredients.
    /// </summary>
    /// <returns>List of recipes</returns>
    public List<Recipe> GetAll()
    {
        _logger.LogInformation("Fetching all recipes");

        return _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .ToList();
    }

    /// <summary>
    /// Retrieves a specific recipe by ID, including ingredients.
    /// </summary>
    /// <param name="id">Recipe ID</param>
    /// <returns>Recipe or null if not found</returns>
    public Recipe? GetById(int id)
    {
        _logger.LogInformation("Fetching recipe with ID: {RecipeId}", id);

        return _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefault(r => r.RecipeId == id);
    }

    /// <summary>
    /// Adds a new recipe to the database.
    /// </summary>
    /// <param name="recipe">Recipe object</param>
    public void Add(Recipe recipe)
    {
        _logger.LogInformation("Adding new recipe: {RecipeName}", recipe.Name);

        _context.Recipes.Add(recipe);
        _context.SaveChanges();
    }

    /// <summary>
    /// Updates an existing recipe.
    /// </summary>
    /// <param name="recipe">Updated recipe object</param>
    public void Update(Recipe recipe)
    {
        _logger.LogInformation("Updating recipe ID: {RecipeId}", recipe.RecipeId);

        _context.Recipes.Update(recipe);
        _context.SaveChanges();
    }

    /// <summary>
    /// Deletes a recipe by ID.
    /// </summary>
    /// <param name="id">Recipe ID</param>
    public void Delete(int id)
    {
        _logger.LogWarning("Deleting recipe with ID: {RecipeId}", id);

        var recipe = _context.Recipes.Find(id);

        if (recipe != null)
        {
            _context.Recipes.Remove(recipe);
            _context.SaveChanges();
        }
    }

    /// <summary>
    /// Adds an ingredient to a recipe with a specified quantity.
    /// </summary>
    /// <param name="recipeId">Recipe ID</param>
    /// <param name="ingredientId">Ingredient ID</param>
    /// <param name="quantity">Quantity used in recipe</param>
    public void AddIngredientToRecipe(int recipeId, int ingredientId, decimal quantity)
    {
        _logger.LogInformation("Adding ingredient {IngredientId} to recipe {RecipeId}", ingredientId, recipeId);

        var recipeIngredient = new RecipeIngredient
        {
            RecipeId = recipeId,
            IngredientId = ingredientId,
            Quantity = quantity
        };

        _context.RecipeIngredients.Add(recipeIngredient);
        _context.SaveChanges();
    }

    /// <summary>
    /// Calculates the total cost of a recipe based on its ingredients.
    /// </summary>
    /// <param name="recipeId">Recipe ID</param>
    /// <returns>Total cost of the recipe</returns>
    public decimal CalculateRecipeCost(int recipeId)
    {
        _logger.LogInformation("Calculating cost for recipe ID: {RecipeId}", recipeId);

        var recipe = _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefault(r => r.RecipeId == recipeId);

        if (recipe == null)
        {
            _logger.LogWarning("Recipe not found for cost calculation");
            return 0;
        }

        decimal totalCost = 0;

        foreach (var ri in recipe.RecipeIngredients)
        {
            if (ri.Ingredient != null)
            {
                totalCost += ri.Quantity * ri.Ingredient.CostPerUnit;
            }
        }

        return totalCost;
    }
}