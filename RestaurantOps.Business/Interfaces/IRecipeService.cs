using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Defines recipe-related business operations.
/// </summary>
public interface IRecipeService
{
    List<Recipe> GetAll();

    Recipe? GetById(int id);

    void Add(Recipe recipe);

    void Update(Recipe recipe);

    void Delete(int id);

    void AddIngredientToRecipe(int recipeId, int ingredientId, decimal quantity);

    decimal CalculateRecipeCost(int recipeId);
}