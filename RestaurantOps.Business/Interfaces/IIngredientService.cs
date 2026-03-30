using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Defines business logic operations for Ingredients.
/// </summary>
public interface IIngredientService
{
    List<Ingredient> GetAll();

    Ingredient? GetById(int id);

    void Add(Ingredient ingredient);

    void Update(Ingredient ingredient);

    void Delete(int id);

    decimal GetTotalInventoryValue();
}