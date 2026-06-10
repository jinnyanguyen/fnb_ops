using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents an individual instructional step within a training module.
/// Steps allow training content to be presented in a structured sequence.
/// </summary>
public class TrainingStep
{
    /// <summary>
    /// Primary key for the TrainingStep table.
    /// </summary>
    public int TrainingStepId { get; set; }

    /// <summary>
    /// Foreign key to the parent training module.
    /// </summary>
    [Required]
    public int TrainingModuleId { get; set; }

    /// <summary>
    /// Navigation property to the parent training module.
    /// </summary>
    [ForeignKey(nameof(TrainingModuleId))]
    public TrainingModule? TrainingModule { get; set; }

    /// <summary>
    /// Display order of the step within the module.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int StepOrder { get; set; }

    /// <summary>
    /// Optional short title for the step.
    /// </summary>
    [StringLength(150)]
    public string? Title { get; set; }

    /// <summary>
    /// Instructional text shown to the staff member.
    /// </summary>
    [Required(ErrorMessage = "Instruction is required.")]
    [StringLength(2000)]
    public string Instruction { get; set; } = string.Empty;

    /// <summary>
    /// Optional supporting video URL for the step.
    /// </summary>
    [StringLength(500)]
    public string? VideoUrl { get; set; }
}