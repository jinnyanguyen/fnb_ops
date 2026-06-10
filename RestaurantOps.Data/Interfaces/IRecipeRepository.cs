using RestaurantOps.Models;

namespace RestaurantOps.Data.Interfaces;

/// <summary>
/// Repository interface for Recipe data access.
/// Responsible only for database operations.
/// </summary>
public interface IRecipeRepository
{
    /// <summary>
    /// Retrieves all recipes.
    /// Used for admin/global reporting.
    /// </summary>
    List<Recipe> GetAll();

    /// <summary>
    /// Retrieves recipes for a specific branch.
    /// </summary>
    List<Recipe> GetAllByBranch(int branchId);

    /// <summary>
    /// Retrieves a recipe by ID.
    /// </summary>
    Recipe? GetById(int id);

    /// <summary>
    /// Adds a new recipe.
    /// </summary>
    void Add(Recipe recipe);

    /// <summary>
    /// Updates an existing recipe.
    /// </summary>
    void Update(Recipe recipe);

    /// <summary>
    /// Deletes a recipe by ID.
    /// </summary>
    void Delete(int id);

    /// <summary>
    /// Saves database changes.
    /// </summary>
    void Save();

/// <summary>
/// Adds a preparation step to a recipe.
/// </summary>
void AddStep(RecipeStep step);
}