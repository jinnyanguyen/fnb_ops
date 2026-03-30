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
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Selling price of the recipe item.
    /// </summary>
    public decimal SellingPrice { get; set; }

    /// <summary>
    /// Category used to group recipes, such as Burger, Pasta, Beverage, etc.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// A recipe has a list of ingredients attached to it
    /// </summary>
    public List<RecipeIngredient> RecipeIngredients { get; set; } = new();
}