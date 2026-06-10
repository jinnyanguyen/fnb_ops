using Microsoft.Extensions.Logging;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data.Interfaces;
using RestaurantOps.Models;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Handles sale-related business logic.
/// </summary>
public class SaleService : ISaleService
{
    private readonly ISaleRepository _repository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly ILogger<SaleService> _logger;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public SaleService(
        ISaleRepository repository,
        IRecipeRepository recipeRepository,
        ILogger<SaleService> logger)
    {
        _repository = repository;
        _recipeRepository = recipeRepository;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all sales.
    /// </summary>
    public List<Sale> GetAll()
    {
        _logger.LogInformation("Fetching all sales");

        return _repository.GetAll();
    }

    /// <summary>
    /// Retrieves sales for a specific branch.
    /// </summary>
    public List<Sale> GetAll(int branchId)
    {
        _logger.LogInformation(
            "Fetching sales for branch ID: {BranchId}",
            branchId);

        return _repository.GetAllByBranch(branchId);
    }

    /// <summary>
    /// Retrieves sale by ID.
    /// </summary>
    public Sale? GetById(int id)
    {
        return _repository.GetById(id);
    }

/// <summary>
/// Adds a new sale.
/// Automatically calculates TotalAmount
/// and deducts ingredient inventory.
/// </summary>
public void Add(Sale sale)
{
    // Load recipe with ingredients
    var recipe = _recipeRepository.GetById(sale.RecipeId);

    if (recipe == null)
    {
        _logger.LogError(
            "Recipe not found for sale creation.");

        throw new Exception("Recipe not found.");
    }

    // Auto-assign sale date
    if (sale.SaleDate == default)
    {
        sale.SaleDate = DateTime.Now;
    }

    // Calculate sale total
    sale.TotalAmount =
        recipe.SellingPrice * sale.QuantitySold;

    // ===== INVENTORY DEDUCTION =====

    foreach (var recipeIngredient in recipe.RecipeIngredients)
    {
        var ingredient = recipeIngredient.Ingredient;

        if (ingredient == null)
        {
            continue;
        }

        // Total inventory deduction
        var deductionAmount =
            recipeIngredient.QuantityRequired *
            sale.QuantitySold;

        // Prevent negative inventory
        if (ingredient.QuantityOnHand < deductionAmount)
        {
            _logger.LogError(
                "Insufficient inventory for ingredient: {IngredientName}",
                ingredient.Name);

            throw new Exception(
                $"Not enough inventory for {ingredient.Name}");
        }

        // Deduct inventory
        ingredient.QuantityOnHand -= deductionAmount;

        _logger.LogInformation(
            "Deducted {Amount} from ingredient {IngredientName}",
            deductionAmount,
            ingredient.Name);
    }

    // Save sale
    _repository.Add(sale);

    // Save inventory updates + sale
    _repository.Save();

    _logger.LogInformation(
        "Sale created successfully with inventory deduction.");
}

    /// <summary>
    /// Updates an existing sale.
    /// </summary>
    public void Update(Sale sale)
    {
        var recipe = _recipeRepository.GetById(sale.RecipeId);

        if (recipe == null)
        {
            _logger.LogError(
                "Recipe not found for sale update.");

            throw new Exception("Recipe not found.");
        }
        if (sale.SaleDate == default)
        {
            sale.SaleDate = DateTime.Now;
        }

        sale.TotalAmount =
            recipe.SellingPrice * sale.QuantitySold;

        _repository.Update(sale);
        _repository.Save();

        _logger.LogInformation(
            "Sale updated successfully.");
    }

    /// <summary>
    /// Deletes a sale by ID.
    /// </summary>
    public void Delete(int id)
    {
        _repository.Delete(id);
        _repository.Save();

        _logger.LogWarning(
            "Sale deleted. ID: {SaleId}",
            id);
    }
}