using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Defines business logic operations for Ingredients.
/// </summary>
public interface IIngredientService
{
    /// <summary>
    /// Retrieves ingredients for a specific branch.
    /// </summary>
    List<Ingredient> GetAll(int branchId);

    /// <summary>
    /// Retrieves ingredient by ID.
    /// </summary>
    Ingredient? GetById(int id);

    /// <summary>
    /// Adds a new ingredient.
    /// </summary>
    void Add(Ingredient ingredient);

    /// <summary>
    /// Updates an ingredient.
    /// </summary>
    void Update(Ingredient ingredient);

    /// <summary>
    /// Deletes an ingredient.
    /// </summary>
    void Delete(int id);

    /// <summary>
    /// Calculates total inventory value.
    /// </summary>
    decimal GetTotalInventoryValue(int branchId);
}