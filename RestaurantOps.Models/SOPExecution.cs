using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents a staff execution of an SOP template.
/// Example:
/// - Opening checklist completed today
/// </summary>
public class SOPExecution
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public int SOPExecutionId { get; set; }

    /// <summary>
    /// SOP template being executed.
    /// </summary>
    [Required]
    public int SOPTemplateId { get; set; }

    /// <summary>
    /// Navigation property to SOP template.
    /// </summary>
    [ForeignKey(nameof(SOPTemplateId))]
    public SOPTemplate? SOPTemplate { get; set; }

    /// <summary>
    /// Staff user executing SOP.
    /// </summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// Navigation property to User.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    /// <summary>
    /// Execution timestamp.
    /// </summary>
    public DateTime ExecutedAt { get; set; } =
        DateTime.Now;

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
    /// Execution checklist items.
    /// </summary>
    public List<SOPExecutionItem> ExecutionItems { get; set; }
        = new();
}