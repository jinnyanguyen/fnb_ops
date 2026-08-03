using Microsoft.EntityFrameworkCore;
using RestaurantOps.Data.Repositories.Interfaces;
using RestaurantOps.Models.Integrations;

namespace RestaurantOps.Data.Repositories;

/// <summary>
/// Provides database access for imported sale records.
/// </summary>
public class ImportedSaleRecordRepository
    : IImportedSaleRecordRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes the repository with the application database context.
    /// </summary>
    /// <param name="context">
    /// Entity Framework Core context used to access import records.
    /// </param>
    public ImportedSaleRecordRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsSuccessfulAsync(
        string sourceSystem,
        string externalSaleId)
    {
        return await _context.ImportedSaleRecords
            .AsNoTracking()
            .AnyAsync(record =>
                record.SourceSystem == sourceSystem &&
                record.ExternalSaleId == externalSaleId &&
                record.IsSuccessful);
    }

    /// <inheritdoc />
    public void Add(ImportedSaleRecord record)
    {
        _context.ImportedSaleRecords.Add(record);
    }

    /// <inheritdoc />
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<ImportedSaleRecord?> GetByExternalIdAsync(
        string sourceSystem,
        string externalSaleId)
    {
        return await _context.ImportedSaleRecords
            .FirstOrDefaultAsync(record =>
                record.SourceSystem == sourceSystem &&
                record.ExternalSaleId == externalSaleId);
    }

    /// <inheritdoc />
    public void Update(ImportedSaleRecord record)
    {
        _context.ImportedSaleRecords.Update(record);
    }
}