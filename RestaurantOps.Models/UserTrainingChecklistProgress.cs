using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Tracks completion status of a checklist item within a specific user training assignment.
/// This entity connects a user’s training session to individual checklist requirements.
/// </summary>
public class UserTrainingChecklistProgress
{
    /// <summary>
    /// Primary key for the checklist progress record.
    /// </summary>
    public int UserTrainingChecklistProgressId { get; set; }

    /// <summary>
    /// Foreign key to the associated user training assignment.
    /// </summary>
    [Required]
    public int UserTrainingId { get; set; }

    /// <summary>
    /// Navigation property to the user training assignment.
    /// </summary>
    [ForeignKey(nameof(UserTrainingId))]
    public UserTraining? UserTraining { get; set; }

    /// <summary>
    /// Foreign key to the related checklist item.
    /// </summary>
    [Required]
    public int TrainingChecklistItemId { get; set; }

    /// <summary>
    /// Navigation property to the checklist item.
    /// </summary>
    [ForeignKey(nameof(TrainingChecklistItemId))]
    public TrainingChecklistItem? TrainingChecklistItem { get; set; }

    /// <summary>
    /// Indicates whether this checklist item has been completed.
    /// </summary>
    public bool IsCompleted { get; set; } = false;

    /// <summary>
    /// Date and time when the checklist item was completed.
    /// Null if not yet completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Optional reference to the user who confirmed completion.
    /// Useful for manager verification scenarios.
    /// </summary>
    public int? ConfirmedByUserId { get; set; }
}