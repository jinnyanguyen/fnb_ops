using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data.Repositories.Interfaces;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Provides business logic for resolving external menu items
/// to internal Gusto Ops recipes.
/// </summary>
public class ExternalRecipeMappingService
    : IExternalRecipeMappingService
{
    private readonly IExternalRecipeMappingRepository _repository;

    /// <summary>
    /// Initializes the service with the external recipe mapping repository.
    /// </summary>
    /// <param name="repository">
    /// Repository used to retrieve active external recipe mappings.
    /// </param>
    public ExternalRecipeMappingService(
        IExternalRecipeMappingRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<int> GetRecipeIdAsync(
        string sourceSystem,
        string externalItemId)
    {
        var mapping = await _repository.GetActiveMappingAsync(
            sourceSystem,
            externalItemId);

        if (mapping == null)
        {
            throw new InvalidOperationException(
                $"No active recipe mapping exists for " +
                $"'{sourceSystem}' item '{externalItemId}'.");
        }

        return mapping.RecipeId;
    }
}