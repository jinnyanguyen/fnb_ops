using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents a manually entered daily sales record for a recipe.
/// </summary>
public class Sale
{
    /// <summary>
    /// Primary key for the Sale table.
    /// </summary>
    public int SaleId { get; set; }

    /// <summary>
    /// Foreign key reference to the related recipe.
    /// </summary>
    [Required(ErrorMessage = "Recipe is required.")]
    public int RecipeId { get; set; }

    /// <summary>
    /// Number of units sold for the recipe.
    /// </summary>
    [Range(1, 100000, ErrorMessage = "Quantity sold must be at least 1.")]
    public int QuantitySold { get; set; }

    /// <summary>
    /// Date the sale occurred.
    /// </summary>
    [Required(ErrorMessage = "Sale date is required.")]
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Total amount is calculated automatically (not user input).
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Navigation property to the related recipe.
    /// </summary>
    public Recipe? Recipe { get; set; }

    [Required]
    public int BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public Branch? Branch { get; set; }
}