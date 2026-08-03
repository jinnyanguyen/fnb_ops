using Microsoft.EntityFrameworkCore;
using RestaurantOps.Data.Repositories.Interfaces;
using RestaurantOps.Models.Integrations;

namespace RestaurantOps.Data.Repositories;

/// <summary>
/// Provides database access for external recipe mappings.
/// </summary>
public class ExternalRecipeMappingRepository
    : IExternalRecipeMappingRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes the repository with the application database context.
    /// </summary>
    /// <param name="context">
    /// Entity Framework Core context used to access mapping records.
    /// </param>
    public ExternalRecipeMappingRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<ExternalRecipeMapping?> GetActiveMappingAsync(
        string sourceSystem,
        string externalItemId)
    {
        return await _context.ExternalRecipeMappings
            .AsNoTracking()
            .FirstOrDefaultAsync(mapping =>
                mapping.SourceSystem == sourceSystem &&
                mapping.ExternalItemId == externalItemId &&
                mapping.IsActive);
    }
}