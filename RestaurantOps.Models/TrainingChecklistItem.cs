using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents a checklist item within a training module.
/// Checklist items are used to confirm that a staff member
/// has completed required actions or understands key concepts.
/// </summary>
public class TrainingChecklistItem
{
    /// <summary>
    /// Primary key for the checklist item.
    /// </summary>
    public int TrainingChecklistItemId { get; set; }

    /// <summary>
    /// Foreign key to the associated training module.
    /// </summary>
    [Required]
    public int TrainingModuleId { get; set; }

    /// <summary>
    /// Navigation property to the parent training module.
    /// </summary>
    [ForeignKey(nameof(TrainingModuleId))]
    public TrainingModule? TrainingModule { get; set; }

    /// <summary>
    /// Descriptive text of the checklist item.
    /// Example: "Wears gloves properly".
    /// </summary>
    [Required]
    [StringLength(300)]
    public string ItemText { get; set; } = string.Empty;

    /// <summary>
    /// Determines the display order of the checklist item.
    /// Lower values appear first.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates whether this checklist item must be completed
    /// before the training can be marked as complete.
    /// </summary>
    public bool IsRequired { get; set; } = true;

    /// <summary>
    /// Navigation property representing user completion records
    /// for this checklist item.
    /// </summary>
    public ICollection<UserTrainingChecklistProgress> ChecklistProgressRecords { get; set; }
        = new List<UserTrainingChecklistProgress>();
}