namespace RestaurantOps.Models;

/// <summary>
/// Represents the overall result of importing sales records
/// from an external point-of-sale source.
/// </summary>
public class PosImportResult
{
    /// <summary>
    /// Total number of data rows read from the import file.
    /// The CSV header row is not included.
    /// </summary>
    public int TotalRows { get; set; }

    /// <summary>
    /// Number of sales records imported successfully.
    /// </summary>
    public int SuccessfulImports { get; set; }

    /// <summary>
    /// Number of sales records that could not be imported.
    /// </summary>
    public int FailedImports { get; set; }

    /// <summary>
    /// Indicates whether every processed row was imported successfully.
    /// </summary>
    public bool IsSuccessful =>
        TotalRows > 0 &&
        FailedImports == 0;

    /// <summary>
    /// Detailed validation or processing errors generated during import.
    /// </summary>
    public List<PosImportError> Errors { get; set; } = new();
}