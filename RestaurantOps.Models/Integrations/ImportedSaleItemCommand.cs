namespace RestaurantOps.Models.Integrations;

/// <summary>
/// Represents one vendor-neutral line item from an imported sale.
/// </summary>
public sealed class ImportedSaleItemCommand
{
    /// <summary>
    /// Product or menu-item identifier supplied by the external source.
    /// </summary>
    public required string ExternalItemId { get; init; }

    /// <summary>
    /// Human-readable item name when supplied by the source.
    /// </summary>
    public string? ItemName { get; init; }

    /// <summary>
    /// Number of units sold.
    /// Decimal is used because restaurant products may support
    /// fractional quantities.
    /// </summary>
    public decimal Quantity { get; init; }

    /// <summary>
    /// Price charged for one unit before line-level adjustments.
    /// </summary>
    public decimal UnitPrice { get; init; }

    /// <summary>
    /// Discount applied to this line.
    /// </summary>
    public decimal DiscountAmount { get; init; }

    /// <summary>
    /// Optional external parent item identifier used for combos,
    /// modifiers, or bundled products.
    /// </summary>
    public string? ParentExternalItemId { get; init; }

    /// <summary>
    /// Indicates whether the source identifies this item as a kit
    /// or bill-of-materials item.
    /// </summary>
    public bool IsKit { get; init; }
}