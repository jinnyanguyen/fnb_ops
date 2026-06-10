using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Represents the relationship between Recipe and Ingredient.
/// </summary>
public class RecipeIngredient
{
    public int RecipeIngredientId { get; set; }

    public int RecipeId { get; set; }

    public int IngredientId { get; set; }

    /// <summary>
    /// Quantity of ingredient used in recipe
    /// </summary>
    [Range(0.01, 10000)]
    public decimal Quantity { get; set; }

    // Navigation properties
    public Recipe? Recipe { get; set; }
    public Ingredient? Ingredient { get; set; }
    public decimal QuantityRequired { get; set; }
}