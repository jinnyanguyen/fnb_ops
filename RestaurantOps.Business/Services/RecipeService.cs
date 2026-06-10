using Microsoft.Extensions.Logging;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data.Interfaces;
using RestaurantOps.Models;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Handles recipe-related business logic.
/// Responsible for:
/// - Recipe validation
/// - Recipe costing
/// - Profit calculations
/// - Ingredient-to-recipe operations
/// 
/// Database access is delegated to repositories.
/// </summary>
public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _repository;
    private readonly IIngredientRepository _ingredientRepository;
    private readonly ILogger<RecipeService> _logger;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    /// <param name="repository">
    /// Recipe repository for database operations.
    /// </param>
    /// <param name="ingredientRepository">
    /// Ingredient repository for ingredient retrieval.
    /// </param>
    /// <param name="logger">
    /// Logger for diagnostics and monitoring.
    /// </param>
    public RecipeService(
        IRecipeRepository repository,
        IIngredientRepository ingredientRepository,
        ILogger<RecipeService> logger)
    {
        _repository = repository;
        _ingredientRepository = ingredientRepository;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all recipes.
    /// Used for admin/global reporting.
    /// </summary>
    public List<Recipe> GetAll()
    {
        _logger.LogInformation("Fetching all recipes");

        return _repository.GetAll();
    }

    /// <summary>
    /// Retrieves recipes for a specific branch.
    /// </summary>
    /// <param name="branchId">
    /// Branch identifier.
    /// </param>
    public List<Recipe> GetAll(int branchId)
    {
        _logger.LogInformation(
            "Fetching recipes for branch ID: {BranchId}",
            branchId);

        return _repository.GetAllByBranch(branchId);
    }

    /// <summary>
    /// Retrieves a recipe by ID.
    /// </summary>
    /// <param name="id">
    /// Recipe identifier.
    /// </param>
    public Recipe? GetById(int id)
    {
        _logger.LogInformation(
            "Fetching recipe with ID: {Id}",
            id);

        return _repository.GetById(id);
    }

    /// <summary>
    /// Adds a new recipe.
    /// </summary>
    /// <param name="recipe">
    /// Recipe object to create.
    /// </param>
    public void Add(Recipe recipe)
    {
        if (recipe.SellingPrice <= 0)
        {
            _logger.LogError(
                "Invalid recipe price for recipe: {Name}",
                recipe.Name);

            throw new ArgumentException(
                "Selling price must be greater than zero.");
        }

        _logger.LogInformation(
            "Adding recipe: {Name}",
            recipe.Name);

        _repository.Add(recipe);
        _repository.Save();
    }

    /// <summary>
    /// Updates an existing recipe.
    /// </summary>
    /// <param name="recipe">
    /// Updated recipe object.
    /// </param>
    public void Update(Recipe recipe)
    {
        if (recipe.SellingPrice <= 0)
        {
            _logger.LogError(
                "Invalid recipe update price for recipe: {Name}",
                recipe.Name);

            throw new ArgumentException(
                "Selling price must be greater than zero.");
        }

        _logger.LogInformation(
            "Updating recipe: {Name}",
            recipe.Name);

        _repository.Update(recipe);
        _repository.Save();
    }

    /// <summary>
    /// Deletes a recipe by ID.
    /// </summary>
    /// <param name="id">
    /// Recipe identifier.
    /// </param>
    public void Delete(int id)
    {
        _logger.LogWarning(
            "Deleting recipe with ID: {Id}",
            id);

        _repository.Delete(id);
        _repository.Save();
    }

    /// <summary>
    /// Adds an ingredient to a recipe.
    /// </summary>
    /// <param name="recipeId">
    /// Recipe identifier.
    /// </param>
    /// <param name="ingredientId">
    /// Ingredient identifier.
    /// </param>
    /// <param name="quantity">
    /// Quantity required for recipe.
    /// </param>
    public void AddIngredientToRecipe(
        int recipeId,
        int ingredientId,
        decimal quantity)
    {
        var recipe = _repository.GetById(recipeId);

        if (recipe == null)
        {
            _logger.LogError(
                "Recipe not found. Recipe ID: {RecipeId}",
                recipeId);

            throw new Exception("Recipe not found.");
        }

        var ingredient = _ingredientRepository.GetById(ingredientId);

        if (ingredient == null)
        {
            _logger.LogError(
                "Ingredient not found. Ingredient ID: {IngredientId}",
                ingredientId);

            throw new Exception("Ingredient not found.");
        }

        recipe.RecipeIngredients.Add(new RecipeIngredient
        {
            RecipeId = recipeId,
            IngredientId = ingredientId,
            QuantityRequired = quantity
        });

        _repository.Update(recipe);
        _repository.Save();

        _logger.LogInformation(
            "Ingredient added to recipe successfully.");
    }

    /// <summary>
    /// Removes an ingredient from a recipe.
    /// </summary>
    /// <param name="recipeId">
    /// Recipe identifier.
    /// </param>
    /// <param name="ingredientId">
    /// Ingredient identifier.
    /// </param>
    public void RemoveIngredientFromRecipe(
        int recipeId,
        int ingredientId)
    {
        var recipe = _repository.GetById(recipeId);

        if (recipe == null)
        {
            _logger.LogError(
                "Recipe not found. Recipe ID: {RecipeId}",
                recipeId);

            throw new Exception("Recipe not found.");
        }

        var recipeIngredient = recipe.RecipeIngredients
            .FirstOrDefault(ri => ri.IngredientId == ingredientId);

        if (recipeIngredient == null)
        {
            _logger.LogError(
                "Ingredient not attached to recipe.");

            throw new Exception(
                "Ingredient not attached to recipe.");
        }

        recipe.RecipeIngredients.Remove(recipeIngredient);

        _repository.Update(recipe);
        _repository.Save();

        _logger.LogInformation(
            "Ingredient removed from recipe successfully.");
    }

    /// <summary>
    /// Calculates the total recipe production cost.
    /// </summary>
    /// <param name="recipeId">
    /// Recipe identifier.
    /// </param>
    public decimal CalculateRecipeCost(int recipeId)
    {
        var recipe = _repository.GetById(recipeId);

        if (recipe == null)
        {
            _logger.LogError(
                "Recipe not found for costing.");

            throw new Exception("Recipe not found.");
        }

        decimal totalCost = 0;

        foreach (var recipeIngredient in recipe.RecipeIngredients)
        {
            var ingredient = _ingredientRepository
                .GetById(recipeIngredient.IngredientId);

            if (ingredient != null)
            {
                totalCost +=
                    ingredient.CostPerUnit *
                    recipeIngredient.QuantityRequired;
            }
        }

        _logger.LogInformation(
            "Calculated recipe cost for recipe ID: {RecipeId}",
            recipeId);

        return totalCost;
    }

    /// <summary>
    /// Calculates recipe profit.
    /// </summary>
    /// <param name="recipeId">
    /// Recipe identifier.
    /// </param>
    public decimal CalculateProfit(int recipeId)
    {
        var recipe = _repository.GetById(recipeId);

        if (recipe == null)
        {
            _logger.LogError(
                "Recipe not found for profit calculation.");

            throw new Exception("Recipe not found.");
        }

        decimal cost = CalculateRecipeCost(recipeId);

        return recipe.SellingPrice - cost;
    }

    /// <summary>
    /// Calculates recipe profit margin percentage.
    /// </summary>
    /// <param name="recipeId">
    /// Recipe identifier.
    /// </param>
    public decimal CalculateProfitMargin(int recipeId)
    {
        var recipe = _repository.GetById(recipeId);

        if (recipe == null)
        {
            _logger.LogError(
                "Recipe not found for margin calculation.");

            throw new Exception("Recipe not found.");
        }

        decimal cost = CalculateRecipeCost(recipeId);

        if (recipe.SellingPrice == 0)
            return 0;

        decimal profit = recipe.SellingPrice - cost;

        return (profit / recipe.SellingPrice) * 100;
    }

    /// <summary>
    /// Adds a preparation step to a recipe.
    /// </summary>
    public void AddStep(RecipeStep step)
    {
        _logger.LogInformation(
            "Adding recipe step to recipe ID: {RecipeId}",
            step.RecipeId);

        var recipe = _repository.GetById(step.RecipeId);

        if (recipe == null)
        {
            _logger.LogError(
                "Recipe not found for step creation. RecipeId: {RecipeId}",
                step.RecipeId);

            throw new Exception("Recipe not found.");
        }

        _repository.AddStep(step);
        _repository.Save();

        _logger.LogInformation(
            "Recipe step added successfully for RecipeId: {RecipeId}",
            step.RecipeId);
    }

    /// <summary>
    /// Retrieves preparation steps for a recipe.
    /// </summary>
    public List<RecipeStep> GetSteps(int recipeId)
    {
        var recipe = _repository.GetById(recipeId);

        if (recipe == null)
        {
            return new List<RecipeStep>();
        }

        return recipe.RecipeSteps
            .OrderBy(s => s.StepOrder)
            .ToList();
    }
}