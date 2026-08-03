namespace RestaurantOps.Business.Services.Interfaces;

/// <summary>
/// Provides business operations for resolving external branch mappings.
/// </summary>
public interface IExternalBranchMappingService
{
    /// <summary>
    /// Resolves an external store identifier to an internal branch ID.
    /// </summary>
    /// <param name="sourceSystem">
    /// External system name (e.g. iPOS).
    /// </param>
    /// <param name="externalStoreId">
    /// External store identifier.
    /// </param>
    /// <returns>
    /// Internal BranchId.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no active mapping exists.
    /// </exception>
    Task<int> GetBranchIdAsync(
        string sourceSystem,
        string externalStoreId);
}