namespace RestaurantOps.Models.Integrations;

/// <summary>
/// Represents the outcome of processing one external sale.
/// </summary>
public sealed class ImportedSaleResult
{
    /// <summary>
    /// Indicates whether the sale was imported successfully.
    /// </summary>
    public bool IsSuccessful { get; init; }

    /// <summary>
    /// Indicates whether the sale was skipped because it was
    /// already imported successfully.
    /// </summary>
    public bool IsSkipped { get; init; }

    /// <summary>
    /// Number of internal sale rows created.
    /// </summary>
    public int SalesCreated { get; init; }

    /// <summary>
    /// Human-readable processing result.
    /// </summary>
    public string Message { get; init; } = string.Empty;
}