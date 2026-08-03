using RestaurantOps.Models.Integrations;

namespace RestaurantOps.Data.Repositories.Interfaces;

/// <summary>
/// Defines data-access operations for imported sale records.
/// These records support duplicate detection and import auditing.
/// </summary>
public interface IImportedSaleRecordRepository
{
    /// <summary>
    /// Retrieves an import record using its external identity.
    /// </summary>
    Task<ImportedSaleRecord?> GetByExternalIdAsync(
        string sourceSystem,
        string externalSaleId);

    /// <summary>
    /// Determines whether an external sale was already imported successfully.
    /// </summary>
    Task<bool> ExistsSuccessfulAsync(
        string sourceSystem,
        string externalSaleId);

    /// <summary>
    /// Adds a new import record.
    /// </summary>
    void Add(ImportedSaleRecord record);

    /// <summary>
    /// Marks an existing import record as modified.
    /// </summary>
    void Update(ImportedSaleRecord record);

    /// <summary>
    /// Saves pending changes.
    /// </summary>
    Task SaveAsync();

    
}