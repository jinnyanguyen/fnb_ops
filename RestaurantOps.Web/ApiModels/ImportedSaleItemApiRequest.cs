using System.ComponentModel.DataAnnotations;

namespace RestaurantOps.Web.ApiModels;

/// <summary>
/// Represents one menu item within an external sale request.
/// </summary>
public sealed class ImportedSaleItemApiRequest
{
    /// <summary>
    /// External menu-item identifier.
    /// This value is resolved through ExternalRecipeMappings.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string ExternalItemId { get; set; } = string.Empty;

    /// <summary>
    /// Optional item name supplied for logging or diagnostics.
    /// </summary>
    [StringLength(200)]
    public string? ItemName { get; set; }

    /// <summary>
    /// Number of units sold.
    /// The current internal Sale model supports whole-number quantities.
    /// </summary>
    [Range(0.01, 100000)]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Unit price supplied by the external system.
    /// The current SaleService calculates revenue using the internal recipe price.
    /// </summary>
    [Range(0, 100000000)]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Discount applied to this item.
    /// Reserved for future financial reconciliation.
    /// </summary>
    [Range(0, 100000000)]
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Optional parent item identifier used for combo components.
    /// </summary>
    public string? ParentExternalItemId { get; set; }

    /// <summary>
    /// Indicates whether the external system identifies the item
    /// as a kit or bill-of-materials item.
    /// </summary>
    public bool IsKit { get; set; }
}