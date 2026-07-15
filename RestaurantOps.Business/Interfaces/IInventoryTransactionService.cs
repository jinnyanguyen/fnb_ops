using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Handles inventory transaction reporting.
/// </summary>
public interface IInventoryTransactionService
{
    /// <summary>
    /// Retrieves inventory transactions
    /// for a specific branch.
    /// </summary>
    List<InventoryTransaction>
        GetByBranch(int branchId);
}