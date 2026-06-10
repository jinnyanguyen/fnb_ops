using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents completion state for recipe steps.
/// </summary>
public class RecipeExecutionStep
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public int RecipeExecutionStepId { get; set; }

    /// <summary>
    /// Related execution session.
    /// </summary>
    [Required]
    public int RecipeExecutionId { get; set; }

    /// <summary>
    /// Navigation property to execution.
    /// </summary>
    [ForeignKey(nameof(RecipeExecutionId))]
    public RecipeExecution? RecipeExecution { get; set; }

    /// <summary>
    /// Related recipe step.
    /// </summary>
    [Required]
    public int RecipeStepId { get; set; }

    /// <summary>
    /// Navigation property to recipe step.
    /// </summary>
    [ForeignKey(nameof(RecipeStepId))]
    public RecipeStep? RecipeStep { get; set; }

    /// <summary>
    /// Indicates whether step is completed.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Completion timestamp.
    /// </summary>
    public DateTime? CompletedAt { get; set; }
}