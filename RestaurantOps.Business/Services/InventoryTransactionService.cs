using Microsoft.Extensions.Logging;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data.Interfaces;
using RestaurantOps.Models;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Handles inventory transaction reporting.
/// </summary>
public class InventoryTransactionService
    : IInventoryTransactionService
{
    private readonly
        IInventoryTransactionRepository
        _repository;

    private readonly
        ILogger<InventoryTransactionService>
        _logger;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public InventoryTransactionService(
        IInventoryTransactionRepository repository,
        ILogger<InventoryTransactionService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves inventory transactions
    /// for a branch.
    /// </summary>
    public List<InventoryTransaction>
        GetByBranch(int branchId)
    {
        _logger.LogInformation(
            "Fetching inventory transactions for branch {BranchId}",
            branchId);

        return _repository.GetByBranch(
            branchId);
    }
}