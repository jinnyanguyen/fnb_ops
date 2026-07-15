using Microsoft.EntityFrameworkCore;
using RestaurantOps.Data.Interfaces;
using RestaurantOps.Models;

namespace RestaurantOps.Data.Repositories;

/// <summary>
/// Handles database operations for
/// Inventory Transactions.
/// </summary>
public class InventoryTransactionRepository
    : IInventoryTransactionRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public InventoryTransactionRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all inventory transactions.
    /// </summary>
    public List<InventoryTransaction> GetAll()
    {
        return _context.InventoryTransactions
            .Include(t => t.Ingredient)
            .OrderByDescending(
                t => t.TransactionDate)
            .ToList();
    }

    /// <summary>
    /// Retrieves inventory transactions
    /// for a specific branch.
    /// </summary>
    public List<InventoryTransaction> GetByBranch(
        int branchId)
    {
        return _context.InventoryTransactions
            .Include(t => t.Ingredient)
            .Where(t =>
                t.BranchId == branchId)
            .OrderByDescending(
                t => t.TransactionDate)
            .ToList();
    }

    /// <summary>
    /// Adds a new inventory transaction.
    /// </summary>
    public void Add(
        InventoryTransaction transaction)
    {
        _context.InventoryTransactions
            .Add(transaction);
    }

    /// <summary>
    /// Saves pending database changes.
    /// </summary>
    public void Save()
    {
        _context.SaveChanges();
    }
}