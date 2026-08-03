using Microsoft.EntityFrameworkCore;
using RestaurantOps.Data.Repositories.Interfaces;
using RestaurantOps.Models.Integrations;

namespace RestaurantOps.Data.Repositories;

/// <summary>
/// Provides database access for external branch mappings.
/// </summary>
public class ExternalBranchMappingRepository
    : IExternalBranchMappingRepository
{
    private readonly ApplicationDbContext _context;

    public ExternalBranchMappingRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<ExternalBranchMapping?> GetActiveMappingAsync(
        string sourceSystem,
        string externalStoreId)
    {
        return await _context.ExternalBranchMappings
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.SourceSystem == sourceSystem &&
                x.ExternalStoreId == externalStoreId &&
                x.IsActive);
    }
}