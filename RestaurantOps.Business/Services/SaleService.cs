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
    private readonly IIngredientRepository _ingredientRepository;
    private readonly ILogger<SaleService> _logger;

    /// <summary>
    /// Repository for inventory audit logging.
    /// </summary>
    private readonly
        IInventoryTransactionRepository
        _inventoryTransactionRepository;

    private readonly ITransactionManager _transactionManager;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public SaleService(
    ISaleRepository repository,
    IRecipeRepository recipeRepository,
    IIngredientRepository ingredientRepository,
    IInventoryTransactionRepository inventoryTransactionRepository,
    ILogger<SaleService> logger,
    ITransactionManager transactionManager)
    {
        _repository = repository;
        _recipeRepository = recipeRepository;
        _ingredientRepository = ingredientRepository;
        _inventoryTransactionRepository =
            inventoryTransactionRepository;
        _logger = logger;
        _transactionManager = transactionManager;
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


    public void Add(Sale sale)
    {
        ArgumentNullException.ThrowIfNull(sale);

        _transactionManager.Execute(
            () => AddInternal(sale));
    }

    /// <summary>
    /// Adds a new sale.
    /// Automatically calculates TotalAmount
    /// and deducts ingredient inventory.
    /// </summary>
    public void AddInternal(Sale sale)
    {
        var recipe =
            _recipeRepository.GetById(sale.RecipeId);

        if (recipe == null)
        {
            _logger.LogError(
                "Recipe not found for sale creation.");

            throw new Exception(
                "Recipe not found.");
        }

        if (sale.SaleDate == default)
        {
            sale.SaleDate = DateTime.Now;
        }

        // =========================================
        // INVENTORY VALIDATION
        // =========================================

        foreach (var recipeIngredient
            in recipe.RecipeIngredients)
        {
            var ingredient =
                _ingredientRepository.GetById(
                    recipeIngredient.IngredientId);

            if (ingredient == null)
            {
                throw new Exception(
                    $"Ingredient not found.");
            }

            decimal quantityNeeded =
                recipeIngredient.QuantityRequired *
                sale.QuantitySold;

            if (ingredient.QuantityOnHand <
                quantityNeeded)
            {
                throw new Exception(
                    $"Insufficient inventory for {ingredient.Name}. " +
                    $"Required: {quantityNeeded}, " +
                    $"Available: {ingredient.QuantityOnHand}");
            }
        }

        // =========================================
        // INVENTORY DEDUCTION
        // =========================================

        foreach (var recipeIngredient
            in recipe.RecipeIngredients)
        {
            var ingredient =
                _ingredientRepository.GetById(
                    recipeIngredient.IngredientId);

            if (ingredient == null)
            {
                continue;
            }

            decimal quantityNeeded =
                recipeIngredient.QuantityRequired *
                sale.QuantitySold;

            ingredient.QuantityOnHand -=
    quantityNeeded;

            _ingredientRepository.Update(
                ingredient);

            _inventoryTransactionRepository.Add(
                new InventoryTransaction
                {
                    IngredientId =
                        ingredient.IngredientId,

                    QuantityChanged =
                        -quantityNeeded,

                    Reason =
                        $"Sale - Recipe: {recipe.Name}",

                    TransactionDate =
                        DateTime.Now,

                    BranchId =
                        sale.BranchId
                });
        }

        // =========================================
        // SALE TOTAL
        // =========================================

        sale.TotalAmount =
            recipe.SellingPrice *
            sale.QuantitySold;

        _repository.Add(sale);

        // Save inventory updates
        _ingredientRepository.Save();

        // Save inventory audit records
        _inventoryTransactionRepository.Save();

        // Save sale record
        _repository.Save();

        _logger.LogInformation(
            "Sale created and inventory deducted. Recipe ID: {RecipeId}",
            sale.RecipeId);
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