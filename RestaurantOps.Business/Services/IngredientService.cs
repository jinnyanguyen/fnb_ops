using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data.Interfaces;
using RestaurantOps.Models;
using Microsoft.Extensions.Logging;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Handles business logic for Ingredient.
/// </summary>
public class IngredientService : IIngredientService
{
    private readonly IIngredientRepository _repository;
    private readonly ILogger<IngredientService> _logger;

    /// <summary>
    /// Constructor with Dependency Injection
    /// </summary>
    public IngredientService(IIngredientRepository repository, ILogger<IngredientService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves ingredients for a specific branch.
    /// </summary>
    public List<Ingredient> GetAll(int branchId)
    {
        _logger.LogInformation(
            "Fetching ingredients for branch ID: {BranchId}",
            branchId
        );

        return _repository.GetAllByBranch(branchId);
    }

    public Ingredient? GetById(int id)
    {
        _logger.LogInformation("Fetching ingredient with ID: {Id}", id);
        return _repository.GetById(id);
    }

    public void Add(Ingredient ingredient)
    {
        if (ingredient.QuantityOnHand < 0 || ingredient.CostPerUnit < 0)
        {
            _logger.LogError("Invalid ingredient values: negative numbers not allowed");
            throw new ArgumentException("Quantity and Cost must be non-negative");
        }

        _logger.LogInformation("Adding ingredient: {Name}", ingredient.Name);

        _repository.Add(ingredient);
        _repository.Save();
    }

    public void Update(Ingredient ingredient)
    {
        if (ingredient.QuantityOnHand < 0 || ingredient.CostPerUnit < 0)
        {
            _logger.LogError("Invalid update: negative values detected");
            throw new ArgumentException("Quantity and Cost must be non-negative");
        }

        _logger.LogInformation("Updating ingredient: {Name}", ingredient.Name);

        _repository.Update(ingredient);
        _repository.Save();
    }

    public void Delete(int id)
    {
        _logger.LogWarning("Deleting ingredient with ID: {Id}", id);

        _repository.Delete(id);
        _repository.Save();
    }

    /// <summary>
    /// Calculates total inventory value for a branch.
    /// </summary>
    public decimal GetTotalInventoryValue(int branchId)
    {
        _logger.LogInformation(
            "Calculating inventory value for branch ID: {BranchId}",
            branchId);

        var ingredients =
            _repository.GetAllByBranch(branchId);

        return ingredients.Sum(i =>
            i.QuantityOnHand * i.CostPerUnit);
    }
}