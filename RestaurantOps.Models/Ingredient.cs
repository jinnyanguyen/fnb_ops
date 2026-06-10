using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents an inventory ingredient used in recipes.
/// </summary>
public class Ingredient
{
    /// <summary>
    /// Primary key for Ingredient table.
    /// </summary>
    public int IngredientId { get; set; }

    /// <summary>
    /// Ingredient name.
    /// Example: Tomato, Pasta, Cheese.
    /// </summary>
    [Required(ErrorMessage = "Ingredient name is required.")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Current inventory quantity available.
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    [Range(0, 999999)]
    public decimal QuantityOnHand { get; set; }

    /// <summary>
    /// Cost per inventory unit.
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    [Range(0, 999999)]
    public decimal CostPerUnit { get; set; }

    /// <summary>
    /// Measurement unit.
    /// Example: kg, g, liters, pcs.
    /// </summary>
    [Required(ErrorMessage = "Unit is required.")]
    [StringLength(20)]
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Minimum stock level before reorder is needed.
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    [Range(0, 999999)]
    public decimal ReorderLevel { get; set; }

    /// <summary>
    /// Foreign key to branch.
    /// Supports multi-branch inventory separation.
    /// </summary>
    [Required]
    public int BranchId { get; set; }

    /// <summary>
    /// Navigation property to branch.
    /// </summary>
    [ForeignKey(nameof(BranchId))]
    public Branch? Branch { get; set; }

    /// <summary>
    /// Relationship to recipes using this ingredient.
    /// </summary>
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        = new List<RecipeIngredient>();
}