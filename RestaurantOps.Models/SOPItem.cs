using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents an individual SOP checklist item.
/// </summary>
public class SOPItem
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public int SOPItemId { get; set; }

    /// <summary>
    /// Checklist instruction.
    /// </summary>
    [Required]
    [StringLength(300)]
    public string Instruction { get; set; } = string.Empty;

    /// <summary>
    /// Display order within checklist.
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Parent SOP template.
    /// </summary>
    [Required]
    public int SOPTemplateId { get; set; }

    /// <summary>
    /// Navigation property to SOPTemplate.
    /// </summary>
    [ForeignKey(nameof(SOPTemplateId))]
    public SOPTemplate? SOPTemplate { get; set; }
}