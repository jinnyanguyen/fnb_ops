using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Defines sale-related business operations.
/// </summary>
public interface ISaleService
{
    /// <summary>
    /// Retrieves all sales.
    /// Used for admin/global reporting.
    /// </summary>
    List<Sale> GetAll();

    /// <summary>
    /// Retrieves sales for a specific branch.
    /// </summary>
    List<Sale> GetAll(int branchId);

    /// <summary>
    /// Retrieves sale by ID.
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
}