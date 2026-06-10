using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Tracks completion progress of individual training steps
/// within a user training assignment.
/// </summary>
public class UserTrainingStepProgress
{
    public int UserTrainingStepProgressId { get; set; }

    [Required]
    public int UserTrainingId { get; set; }

    [ForeignKey(nameof(UserTrainingId))]
    public UserTraining? UserTraining { get; set; }

    [Required]
    public int TrainingStepId { get; set; }

    [ForeignKey(nameof(TrainingStepId))]
    public TrainingStep? TrainingStep { get; set; }

    public bool IsCompleted { get; set; } = false;

    public DateTime? CompletedAt { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }
}