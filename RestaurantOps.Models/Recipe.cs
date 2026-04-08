using System.ComponentModel.DataAnnotations;

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
    public string Name { get; set; }
    
    /// <summary>
    /// Selling price of the recipe item.
    /// </summary>
    [Range(0.01, 100000, ErrorMessage = "Selling price must be greater than 0")]
    public decimal SellingPrice { get; set; }

    /// <summary>
    /// Category used to group recipes, such as Burger, Pasta, Beverage, etc.
    /// </summary>
    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// A recipe has a list of ingredients attached to it
    /// </summary>
    public List<RecipeIngredient> RecipeIngredients { get; set; } = new();
}
