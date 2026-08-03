namespace RestaurantOps.Integrations.Models;

/// <summary>
/// Describes the outcome of a sales-import operation.
/// The same result model can be returned by CSV, API,
/// webhook, and future POS importers.
/// </summary>
public sealed class ImportResult
{
    /// <summary>
    /// Number of records successfully imported.
    /// </summary>
    public int Imported { get; set; }

    /// <summary>
    /// Number of records intentionally skipped, for example,
    /// because they had already been imported.
    /// </summary>
    public int Skipped { get; set; }

    /// <summary>
    /// Number of records that could not be imported.
    /// </summary>
    public int Failed { get; set; }

    /// <summary>
    /// Human-readable validation or processing errors.
    /// </summary>
    public List<string> Errors { get; set; } = [];
}