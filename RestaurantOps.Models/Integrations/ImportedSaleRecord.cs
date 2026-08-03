using System.ComponentModel.DataAnnotations;

namespace RestaurantOps.Models.Integrations;

/// <summary>
/// Tracks imported external sales to prevent duplicate processing.
/// </summary>
public sealed class ImportedSaleRecord
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public int ImportedSaleRecordId { get; set; }

    /// <summary>
    /// External system that supplied the sale.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string SourceSystem { get; set; } = string.Empty;

    /// <summary>
    /// External sale identifier supplied by the source system.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string ExternalSaleId { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the sale was successfully imported.
    /// </summary>
    public DateTime ImportedAtUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indicates whether the import completed successfully.
    /// </summary>
    public bool IsSuccessful { get; set; }
    

    /// <summary>
    /// Optional message for logging import failures or notes.
    /// </summary>
    [StringLength(1000)]
    public string? Message { get; set; }
}