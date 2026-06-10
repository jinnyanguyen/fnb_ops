using Microsoft.EntityFrameworkCore;
using RestaurantOps.Data.Interfaces;
using RestaurantOps.Models;

namespace RestaurantOps.Data.Repositories;

/// <summary>
/// Handles Recipe database operations.
/// Responsible only for data access.
/// </summary>
public class RecipeRepository : IRecipeRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public RecipeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all recipes.
    /// </summary>
    public List<Recipe> GetAll()
    {
        return _context.Recipes
            .Include(r => r.RecipeIngredients)
            .ToList();
    }

    /// <summary>
    /// Retrieves recipes for a specific branch.
    /// </summary>
    public List<Recipe> GetAllByBranch(int branchId)
    {
        return _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)

            // IMPORTANT:
            .Include(r => r.RecipeSteps)

            .Where(r => r.BranchId == branchId)
            .ToList();
    }

    /// <summary>
    /// Retrieves recipe by ID.
    /// </summary>
    public Recipe? GetById(int id)
    {
        return _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)

            // IMPORTANT:
            .Include(r => r.RecipeSteps)

            .FirstOrDefault(r => r.RecipeId == id);
    }

    /// <summary>
    /// Adds a new recipe.
    /// </summary>
    public void Add(Recipe recipe)
    {
        _context.Recipes.Add(recipe);
    }

    /// <summary>
    /// Updates an existing recipe.
    /// </summary>
    public void Update(Recipe recipe)
    {
        _context.Recipes.Update(recipe);
    }

    /// <summary>
    /// Deletes recipe by ID.
    /// </summary>
    public void Delete(int id)
    {
        var recipe = _context.Recipes.Find(id);

        if (recipe != null)
        {
            _context.Recipes.Remove(recipe);
        }
    }

    /// <summary>
    /// Saves database changes.
    /// </summary>
    public void Save()
    {
        _context.SaveChanges();
    }

    /// <summary>
    /// Adds a preparation step directly to the RecipeSteps table.
    /// </summary>
    public void AddStep(RecipeStep step)
    {
        _context.RecipeSteps.Add(step);
    }
}