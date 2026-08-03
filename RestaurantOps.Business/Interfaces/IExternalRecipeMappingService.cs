namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Defines business operations for resolving external recipe mappings.
/// </summary>
public interface IExternalRecipeMappingService
{
    /// <summary>
    /// Resolves an external menu-item identifier to an internal recipe ID.
    /// </summary>
    /// <param name="sourceSystem">
    /// Name of the external source, such as IPOS or CSV.
    /// </param>
    /// <param name="externalItemId">
    /// Menu-item identifier supplied by the external source.
    /// </param>
    /// <returns>
    /// The internal Gusto Ops RecipeId.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no active recipe mapping exists.
    /// </exception>
    Task<int> GetRecipeIdAsync(
        string sourceSystem,
        string externalItemId);
}