using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models.Integrations;

/// <summary>
/// Maps an external POS menu item to a Gusto Ops recipe.
/// </summary>
public sealed class ExternalRecipeMapping
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public int ExternalRecipeMappingId { get; set; }

    /// <summary>
    /// External system name.
    /// Example: IPOS, CSV, TOAST.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string SourceSystem { get; set; } = string.Empty;

    /// <summary>
    /// Menu item identifier supplied by the external system.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string ExternalItemId { get; set; } = string.Empty;

    /// <summary>
    /// Internal recipe.
    /// </summary>
    [Required]
    public int RecipeId { get; set; }

    /// <summary>
    /// Navigation property.
    /// </summary>
    [ForeignKey(nameof(RecipeId))]
    public Recipe? Recipe { get; set; }

    /// <summary>
    /// Indicates whether the mapping is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// UTC timestamp when the mapping was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}