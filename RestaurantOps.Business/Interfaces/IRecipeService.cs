using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Defines recipe-related business operations.
/// </summary>
public interface IRecipeService
{
    List<Recipe> GetAll();

    /// <summary>
    /// Retrieves recipes for a specific branch.
    /// </summary>
    List<Recipe> GetAll(int branchId);

    Recipe? GetById(int id);

    void Add(Recipe recipe);

    void Update(Recipe recipe);

    void Delete(int id);

    void AddIngredientToRecipe(int recipeId, int ingredientId, decimal quantity);

    decimal CalculateRecipeCost(int recipeId);

    void RemoveIngredientFromRecipe(int recipeId, int ingredientId);

    /// <summary>
    /// Calculates profit for a recipe.
    /// </summary>
    decimal CalculateProfit(int recipeId);

    /// <summary>
    /// Calculates profit margin percentage.
    /// </summary>
    decimal CalculateProfitMargin(int recipeId);

    /// <summary>
    /// Adds a preparation step to a recipe.
    /// </summary>
    void AddStep(RecipeStep step);

    /// <summary>
    /// Retrieves preparation steps for a recipe.
    /// </summary>
    List<RecipeStep> GetSteps(int recipeId);
}