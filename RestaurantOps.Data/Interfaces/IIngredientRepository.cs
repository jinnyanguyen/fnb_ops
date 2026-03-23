using RestaurantOps.Models;

namespace RestaurantOps.Data.Interfaces;

/// <summary>
/// Defines data access operations for Ingredient.
/// </summary>
public interface IIngredientRepository
{
    List<Ingredient> GetAll();

    Ingredient? GetById(int id);

    void Add(Ingredient ingredient);

    void Update(Ingredient ingredient);

    void Delete(int id);

    void Save();
}