using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace RestaurantOps.Models;

/// <summary>
/// Represents an inventory ingredient used by the restaurant.
/// This entity will be stored in the database.
/// </summary>
public class Ingredient
{
    /// <summary>
    /// Primary key for the Ingredient table.
    /// </summary>
    public int IngredientId { get; set; }

    /// <summary>
    /// Ingredient display name, such as Chicken, Flour, or Tomato.
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Unit of measurement, such as lb, kg, liter, or each.
    /// </summary>
    [Required(ErrorMessage = "Unit is required")]
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Quantity currently available in inventory.
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal QuantityOnHand { get; set; }

    /// <summary>
    /// Cost of one unit of this ingredient.
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal CostPerUnit { get; set; }

    /// <summary>
    /// Threshold level used to identify when the ingredient is low in stock.
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal ReorderLevel { get; set; }
}