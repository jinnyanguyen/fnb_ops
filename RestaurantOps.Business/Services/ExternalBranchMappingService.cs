using RestaurantOps.Business.Services.Interfaces;
using RestaurantOps.Data.Repositories.Interfaces;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Provides business logic for resolving external branch mappings.
/// </summary>
public class ExternalBranchMappingService
    : IExternalBranchMappingService
{
    private readonly IExternalBranchMappingRepository _repository;


    /// <summary>
    /// Initializes the service with the external branch mapping repository.
    /// </summary>
    /// <param name="repository">
    /// Repository used to retrieve active external branch mappings.
    /// </param>
    public ExternalBranchMappingService(
        IExternalBranchMappingRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<int> GetBranchIdAsync(
        string sourceSystem,
        string externalStoreId)
    {
        var mapping = await _repository.GetActiveMappingAsync(
            sourceSystem,
            externalStoreId);

        if (mapping == null)
        {
            throw new InvalidOperationException(
                $"No active branch mapping exists for " +
                $"'{sourceSystem}' store '{externalStoreId}'.");
        }

        return mapping.BranchId;
    }
}