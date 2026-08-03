namespace RestaurantOps.Models.Integrations;

/// <summary>
/// Represents a vendor-neutral sale that is ready to enter
/// the Gusto Ops business workflow.
///
/// CSV files, iPOS responses, and future POS providers must
/// all map their source data into this model.
/// </summary>
public sealed class ImportedSaleCommand
{
    /// <summary>
    /// Identifies the source that supplied the sale.
    /// Examples: "CSV", "IPOS", "TOAST".
    /// </summary>
    public required string SourceSystem { get; init; }

    /// <summary>
    /// Unique sale identifier supplied by the source system.
    /// Used to prevent duplicate imports.
    /// </summary>
    public required string ExternalSaleId { get; init; }

    /// <summary>
    /// External branch or store identifier.
    /// This will later be mapped to a Gusto Ops branch.
    /// </summary>
    public required string ExternalStoreId { get; init; }

    /// <summary>
    /// Date and time when the sale occurred.
    /// DateTimeOffset is used because external systems may
    /// provide timezone-aware timestamps.
    /// </summary>
    public DateTimeOffset SaleDate { get; init; }

    /// <summary>
    /// Indicates whether the source considers this record
    /// a creation, update, or deletion.
    /// </summary>
    public ImportedSaleAction Action { get; init; } =
        ImportedSaleAction.Create;

    /// <summary>
    /// Items included in the sale.
    /// Initialized to an empty collection to prevent null errors.
    /// </summary>
    public IReadOnlyCollection<ImportedSaleItemCommand> Items { get; init; } =
        Array.Empty<ImportedSaleItemCommand>();
}