using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents a task assigned to a user.
/// </summary>
public class TaskAssignment
{
    /// <summary>
    /// Primary key
    /// </summary>
    public int TaskAssignmentId { get; set; }

    /// <summary>
    /// Task title or description
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed task description
    /// </summary>
    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Due date for the task
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Current status (Pending, In Progress, Completed)
    /// </summary>
    [Required]
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Timestamp when task was completed.
    /// Null if task is not completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Indicates whether the task is overdue.
    /// Computed dynamically from DueDate and Status.
    /// Not stored in database.
    /// </summary>
    [NotMapped]
    public bool IsOverdue =>
        Status != "Completed" &&
        DueDate < DateTime.Now;

    /// <summary>
    /// User assigned to the task
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Navigation property to User
    /// </summary>
    public User? User { get; set; }

    [Required]
    public int BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public Branch? Branch { get; set; }
}