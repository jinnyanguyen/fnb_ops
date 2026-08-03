namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Defines business operations for duplicate detection
/// and external sale import tracking.
/// </summary>
public interface IImportedSaleRecordService
{
    /// <summary>
    /// Determines whether an external sale has already been
    /// processed successfully.
    /// </summary>
    Task<bool> HasBeenImportedAsync(
        string sourceSystem,
        string externalSaleId);

    /// <summary>
    /// Records the outcome of an external sale import.
    /// </summary>
    Task RecordImportAsync(
        string sourceSystem,
        string externalSaleId,
        bool isSuccessful,
        string? message = null);
}