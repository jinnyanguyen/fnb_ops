using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents a menu recipe or sellable food item.
/// </summary>
public class Recipe
{
    /// <summary>
    /// Primary key for the Recipe table.
    /// </summary>
    public int RecipeId { get; set; }

    /// <summary>
    /// Recipe or menu item name.
    /// </summary>
    [Required(ErrorMessage = "Recipe name is required")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Selling price of the recipe item.
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, 100000, ErrorMessage = "Selling price must be greater than 0")]
    public decimal SellingPrice { get; set; }

    /// <summary>
    /// Category used to group recipes, such as Burger, Pasta, Beverage, etc.
    /// </summary>
    [Required(ErrorMessage = "Category is required")]
    [StringLength(200)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// A recipe has a list of ingredients attached to it
    /// </summary>
    public List<RecipeIngredient> RecipeIngredients { get; set; } = new();

    /// <summary>
    /// Branch that owns this recipe.
    /// Used for multi-branch filtering.
    /// </summary>
    [Required]
    public int BranchId { get; set; }

    /// <summary>
    /// Navigation property to branch.
    /// </summary>
    [ForeignKey(nameof(BranchId))]
    public Branch? Branch { get; set; }

    /// <summary>
    /// Recipe preparation instructions.
    /// </summary>
    [StringLength(2000)]
    public string? Instructions { get; set; }

    /// <summary>
    /// Estimated preparation time in minutes.
    /// </summary>
    public int PrepTimeMinutes { get; set; }

    /// <summary>
    /// Recipe preparation steps.
    /// </summary>
    public List<RecipeStep> RecipeSteps { get; set; } = new();
}
