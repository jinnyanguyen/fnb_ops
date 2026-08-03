using System.ComponentModel.DataAnnotations;

namespace RestaurantOps.Web.ApiModels;

/// <summary>
/// Represents an external sale submitted to the Gusto Ops REST API.
/// This model belongs to the Web layer because it defines the HTTP request.
/// </summary>
public sealed class ImportedSaleApiRequest
{
    /// <summary>
    /// Name of the system supplying the sale.
    /// Examples: IPOS, CSV, TEST-POS.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string SourceSystem { get; set; } = string.Empty;

    /// <summary>
    /// Unique sale or invoice identifier from the external system.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string ExternalSaleId { get; set; } = string.Empty;

    /// <summary>
    /// Store identifier supplied by the external system.
    /// This value is resolved through ExternalBranchMappings.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string ExternalStoreId { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the external sale occurred.
    /// </summary>
    [Required]
    public DateTimeOffset SaleDate { get; set; }

    /// <summary>
    /// Synchronization action.
    /// Current supported value: Create.
    /// </summary>
    public string Action { get; set; } = "Create";

    /// <summary>
    /// Menu items included in the external sale.
    /// </summary>
    [Required]
    [MinLength(1, ErrorMessage = "At least one sale item is required.")]
    public List<ImportedSaleItemApiRequest> Items { get; set; } = [];
}