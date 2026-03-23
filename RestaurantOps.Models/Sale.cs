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
    public int RecipeId { get; set; }

    /// <summary>
    /// Number of units sold for the recipe.
    /// </summary>
    public int QuantitySold { get; set; }

    /// <summary>
    /// Date the sale occurred.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Total amount for the sale entry.
    /// </summary>
    public decimal TotalAmount { get; set; }
}