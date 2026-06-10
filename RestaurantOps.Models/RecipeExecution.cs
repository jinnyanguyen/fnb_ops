using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents a live recipe execution session.
/// </summary>
public class RecipeExecution
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public int RecipeExecutionId { get; set; }

    /// <summary>
    /// Related recipe.
    /// </summary>
    [Required]
    public int RecipeId { get; set; }

    /// <summary>
    /// Navigation property to Recipe.
    /// </summary>
    [ForeignKey(nameof(RecipeId))]
    public Recipe? Recipe { get; set; }

    /// <summary>
    /// Staff member executing recipe.
    /// </summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// Navigation property to User.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    /// <summary>
    /// Execution start time.
    /// </summary>
    public DateTime StartedAt { get; set; }
        = DateTime.Now;

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
    /// Execution steps.
    /// </summary>
    public List<RecipeExecutionStep> ExecutionSteps { get; set; }
        = new();
}