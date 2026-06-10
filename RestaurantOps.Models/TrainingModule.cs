using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents a reusable training module created by management.
/// Training modules are typically targeted to a specific role,
/// such as Cook, Cashier, or Trainee.
/// </summary>
public class TrainingModule
{
    /// <summary>
    /// Primary key for the TrainingModule table.
    /// </summary>
    public int TrainingModuleId { get; set; }

    /// <summary>
    /// Title of the training module.
    /// Example: "Pasta Station Basics".
    /// </summary>
    [Required(ErrorMessage = "Training title is required.")]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the module's purpose and learning outcome.
    /// </summary>
    [StringLength(1000)]
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// Intended staff role for this training module.
    /// Example: Cook, Cashier, Staff, Trainee.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string RoleTarget { get; set; } = "Staff";

    /// <summary>
    /// Optional URL to training content, such as a video or reference guide.
    /// </summary>
    [StringLength(500)]
    public string? ContentUrl { get; set; } = string.Empty;

    /// <summary>
    /// Estimated completion time in minutes.
    /// </summary>
    [Range(1, 600)]
    public int EstimatedMinutes { get; set; }

    /// <summary>
    /// Foreign key to the branch that owns this module.
    /// </summary>
    public int BranchId { get; set; }

    /// <summary>
    /// Navigation property to the branch that owns this training module.
    /// </summary>
    [ForeignKey(nameof(BranchId))]
    public Branch? Branch { get; set; }

    /// <summary>
    /// Date/time when the module was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Ordered list of instructional steps for this module.
    /// </summary>
    public ICollection<TrainingStep> Steps { get; set; } = new List<TrainingStep>();

    /// <summary>
    /// Checklist items used to confirm understanding or readiness.
    /// </summary>
    public ICollection<TrainingChecklistItem> ChecklistItems { get; set; } = new List<TrainingChecklistItem>();

    /// <summary>
    /// Assignment records connecting users to this module.
    /// </summary>
    public ICollection<UserTraining> UserTrainings { get; set; } = new List<UserTraining>();
}