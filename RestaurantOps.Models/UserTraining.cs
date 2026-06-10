using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents the assignment of a training module to a specific user.
/// This table is the bridge between staff and training content.
/// </summary>
public class UserTraining
{
    /// <summary>
    /// Primary key for the UserTraining table.
    /// </summary>
    public int UserTrainingId { get; set; }

    /// <summary>
    /// Foreign key to the assigned user.
    /// </summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// Navigation property to the assigned user.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    /// <summary>
    /// Foreign key to the assigned training module.
    /// </summary>
    [Required]
    public int TrainingModuleId { get; set; }

    /// <summary>
    /// Navigation property to the assigned training module.
    /// </summary>
    [ForeignKey(nameof(TrainingModuleId))]
    public TrainingModule? TrainingModule { get; set; }

    /// <summary>
    /// Date and time when the training was assigned.
    /// </summary>
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the user completed the training.
    /// Null means it is still incomplete.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Current assignment status.
    /// Suggested values: Assigned, InProgress, Completed.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Assigned";

    /// <summary>
    /// Step-level progress records for this training assignment.
    /// </summary>
    public ICollection<UserTrainingStepProgress> StepProgress { get; set; } = new List<UserTrainingStepProgress>();

    /// <summary>
    /// Checklist-level progress records for this training assignment.
    /// </summary>
    public ICollection<UserTrainingChecklistProgress> ChecklistProgress { get; set; } = new List<UserTrainingChecklistProgress>();
}