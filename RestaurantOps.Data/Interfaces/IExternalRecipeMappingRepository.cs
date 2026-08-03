using RestaurantOps.Models.Integrations;

namespace RestaurantOps.Data.Repositories.Interfaces;

/// <summary>
/// Defines data-access operations for external recipe mappings.
/// </summary>
public interface IExternalRecipeMappingRepository
{
    /// <summary>
    /// Retrieves an active mapping for an external menu item.
    /// </summary>
    /// <param name="sourceSystem">
    /// Name of the external source, such as IPOS or CSV.
    /// </param>
    /// <param name="externalItemId">
    /// Menu-item identifier supplied by the external source.
    /// </param>
    /// <returns>
    /// The active recipe mapping when found; otherwise, null.
    /// </returns>
    Task<ExternalRecipeMapping?> GetActiveMappingAsync(
        string sourceSystem,
        string externalItemId);
}