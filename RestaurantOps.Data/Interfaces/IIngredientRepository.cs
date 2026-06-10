using RestaurantOps.Models;

namespace RestaurantOps.Data.Interfaces;

/// <summary>
/// Repository interface responsible for
/// Ingredient database operations.
/// </summary>
public interface IIngredientRepository
{
    /// <summary>
    /// Retrieves all ingredients.
    /// Typically used for administrative
    /// or reporting operations.
    /// </summary>
    /// <returns>
    /// Complete list of ingredients.
    /// </returns>
    List<Ingredient> GetAll();

    /// <summary>
    /// Retrieves ingredients belonging
    /// to a specific branch.
    /// </summary>
    /// <param name="branchId">
    /// Branch identifier.
    /// </param>
    /// <returns>
    /// List of ingredients for the branch.
    /// </returns>
    List<Ingredient> GetAllByBranch(int branchId);

    /// <summary>
    /// Retrieves a single ingredient by ID.
    /// </summary>
    /// <param name="id">
    /// Ingredient identifier.
    /// </param>
    /// <returns>
    /// Ingredient if found; otherwise null.
    /// </returns>
    Ingredient? GetById(int id);

    /// <summary>
    /// Adds a new ingredient to the database.
    /// </summary>
    /// <param name="ingredient">
    /// Ingredient entity to add.
    /// </param>
    void Add(Ingredient ingredient);

    /// <summary>
    /// Updates an existing ingredient.
    /// </summary>
    /// <param name="ingredient">
    /// Ingredient entity to update.
    /// </param>
    void Update(Ingredient ingredient);

    /// <summary>
    /// Deletes an ingredient by ID.
    /// </summary>
    /// <param name="id">
    /// Ingredient identifier.
    /// </param>
    void Delete(int id);

    /// <summary>
    /// Persists pending database changes.
    /// </summary>
    void Save();
}