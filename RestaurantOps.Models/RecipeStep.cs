using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents a preparation step for a recipe.
/// </summary>
public class RecipeStep
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public int RecipeStepId { get; set; }

    /// <summary>
    /// Step instruction.
    /// </summary>
    [Required]
    [StringLength(500)]
    public string Instruction { get; set; } = string.Empty;

    /// <summary>
    /// Step order.
    /// </summary>
    public int StepOrder { get; set; }

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
}