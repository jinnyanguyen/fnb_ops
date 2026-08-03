using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models.Integrations;

/// <summary>
/// Maps an external POS store identifier to an internal Gusto Ops branch.
/// </summary>
public sealed class ExternalBranchMapping
{
    /// <summary>
    /// Primary key for the external branch mapping table.
    /// </summary>
    public int ExternalBranchMappingId { get; set; }

    /// <summary>
    /// Name of the external system that owns the store identifier.
    /// Examples: IPOS, CSV, TOAST.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string SourceSystem { get; set; } = string.Empty;

    /// <summary>
    /// Store identifier supplied by the external system.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string ExternalStoreId { get; set; } = string.Empty;

    /// <summary>
    /// Internal Gusto Ops branch associated with the external store.
    /// </summary>
    [Required]
    public int BranchId { get; set; }

    /// <summary>
    /// Navigation property to the internal branch.
    /// </summary>
    [ForeignKey(nameof(BranchId))]
    public Branch? Branch { get; set; }

    /// <summary>
    /// Indicates whether this mapping may currently be used.
    /// Disabling a mapping is safer than deleting historical configuration.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// UTC timestamp showing when the mapping was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}