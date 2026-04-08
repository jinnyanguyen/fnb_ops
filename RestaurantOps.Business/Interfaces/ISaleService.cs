using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Defines business operations for sales.
/// </summary>
public interface ISaleService
{
    /// <summary>
    /// Returns all sales records, including related recipe data.
    /// </summary>
    List<Sale> GetAll();

    /// <summary>
    /// Adds a new sale record and calculates its total amount.
    /// </summary>
    void Add(Sale sale);
}