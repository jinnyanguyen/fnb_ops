using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents a reusable SOP checklist template.
/// Example:
/// - Opening Checklist
/// - Closing Checklist
/// - Food Safety Checklist
/// </summary>
public class SOPTemplate
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public int SOPTemplateId { get; set; }

    /// <summary>
    /// SOP checklist name.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional SOP description.
    /// </summary>
    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Branch ownership.
    /// </summary>
    [Required]
    public int BranchId { get; set; }

    /// <summary>
    /// Navigation property to Branch.
    /// </summary>
    [ForeignKey(nameof(BranchId))]
    public Branch? Branch { get; set; }

    /// <summary>
    /// SOP checklist items.
    /// </summary>
    public List<SOPItem> SOPItems { get; set; } = new();
}