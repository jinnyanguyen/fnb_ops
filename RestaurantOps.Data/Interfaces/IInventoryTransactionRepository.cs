using RestaurantOps.Models;

namespace RestaurantOps.Data.Interfaces;

/// <summary>
/// Repository interface responsible for
/// Inventory Transaction database operations.
/// Provides audit trail retrieval and persistence.
/// </summary>
public interface IInventoryTransactionRepository
{
    /// <summary>
    /// Retrieves all inventory transactions.
    /// Typically used for administrative reporting.
    /// </summary>
    List<InventoryTransaction> GetAll();

    /// <summary>
    /// Retrieves inventory transactions
    /// for a specific branch.
    /// </summary>
    /// <param name="branchId">
    /// Branch identifier.
    /// </param>
    List<InventoryTransaction> GetByBranch(
        int branchId);

    /// <summary>
    /// Adds a new inventory transaction.
    /// </summary>
    /// <param name="transaction">
    /// Inventory transaction entity.
    /// </param>
    void Add(
        InventoryTransaction transaction);

    /// <summary>
    /// Persists pending database changes.
    /// </summary>
    void Save();
}