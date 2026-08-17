namespace RestaurantOps.Integrations.Contracts;

/// <summary>
/// Represents one row from an externally supplied sales CSV file.
/// This model mirrors the CSV structure only and does not contain
/// database or business-processing logic.
/// </summary>
public sealed class CsvSaleRecord
{
    public string SourceSystem { get; set; } = string.Empty;

    public string ExternalSaleId { get; set; } = string.Empty;

    public string ExternalStoreId { get; set; } = string.Empty;

    public string SaleDate { get; set; } = string.Empty;

    public string ExternalItemId { get; set; } = string.Empty;

    public string? ItemName { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountAmount { get; set; }
}