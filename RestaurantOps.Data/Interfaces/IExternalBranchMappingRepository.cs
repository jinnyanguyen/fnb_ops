using RestaurantOps.Models.Integrations;

namespace RestaurantOps.Data.Repositories.Interfaces;

/// <summary>
/// Provides data access for external branch mappings.
/// </summary>
public interface IExternalBranchMappingRepository
{
    /// <summary>
    /// Retrieves an active branch mapping for an external store.
    /// </summary>
    /// <param name="sourceSystem">
    /// Name of the external system (e.g. ToastPOS, Square, CSV).
    /// </param>
    /// <param name="externalStoreId">
    /// External store identifier.
    /// </param>
    /// <returns>
    /// Matching mapping if found; otherwise null.
    /// </returns>
    Task<ExternalBranchMapping?> GetActiveMappingAsync(
        string sourceSystem,
        string externalStoreId);
}