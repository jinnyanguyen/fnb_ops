using RestaurantOps.Models.Integrations;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Defines the shared business workflow for processing
/// vendor-neutral external sales.
/// </summary>
public interface IImportedSaleService
{
    /// <summary>
    /// Processes one external sale using the existing sales,
    /// inventory-deduction, and audit-log business logic.
    /// </summary>
    Task<ImportedSaleResult> ProcessAsync(
        ImportedSaleCommand command);
}