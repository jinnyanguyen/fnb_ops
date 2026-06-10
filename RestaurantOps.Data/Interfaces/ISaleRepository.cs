using RestaurantOps.Models;

namespace RestaurantOps.Data.Interfaces;

/// <summary>
/// Repository interface for Sale data access.
/// Responsible only for database operations.
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Retrieves all sales.
    /// Used for admin/global reporting.
    /// </summary>
    List<Sale> GetAll();

    /// <summary>
    /// Retrieves sales for a specific branch.
    /// </summary>
    List<Sale> GetAllByBranch(int branchId);

    /// <summary>
    /// Retrieves a sale by ID.
    /// </summary>
    Sale? GetById(int id);

    /// <summary>
    /// Adds a new sale.
    /// </summary>
    void Add(Sale sale);

    /// <summary>
    /// Updates an existing sale.
    /// </summary>
    void Update(Sale sale);

    /// <summary>
    /// Deletes a sale by ID.
    /// </summary>
    void Delete(int id);

    /// <summary>
    /// Saves database changes.
    /// </summary>
    void Save();
}