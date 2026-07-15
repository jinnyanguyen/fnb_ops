using System.ComponentModel.DataAnnotations;

namespace RestaurantOps.Web.ViewModels;

/// <summary>
/// Used for inventory stock adjustments.
/// </summary>
public class InventoryAdjustmentViewModel
{
    public int IngredientId { get; set; }

    public string IngredientName
    {
        get;
        set;
    } = string.Empty;

    [Required]
    public decimal QuantityToAdd
    {
        get;
        set;
    }

    [Required]
    public string Reason
    {
        get;
        set;
    } = "Stock Refill";
}

