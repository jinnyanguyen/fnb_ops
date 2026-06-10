using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents completion state for a checklist item.
/// </summary>
public class SOPExecutionItem
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public int SOPExecutionItemId { get; set; }

    /// <summary>
    /// Related SOP execution.
    /// </summary>
    [Required]
    public int SOPExecutionId { get; set; }

    /// <summary>
    /// Navigation property to execution.
    /// </summary>
    [ForeignKey(nameof(SOPExecutionId))]
    public SOPExecution? SOPExecution { get; set; }

    /// <summary>
    /// Related SOP item.
    /// </summary>
    [Required]
    public int SOPItemId { get; set; }

    /// <summary>
    /// Navigation property to SOP item.
    /// </summary>
    [ForeignKey(nameof(SOPItemId))]
    public SOPItem? SOPItem { get; set; }

    /// <summary>
    /// Indicates whether checklist item is completed.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Completion timestamp.
    /// </summary>
    public DateTime? CompletedAt { get; set; }
}