namespace RestaurantOps.Web.ViewModels;

/// <summary>
/// Most consumed ingredients.
/// </summary>
public class MostUsedIngredientViewModel
{
    public string IngredientName
    {
        get;
        set;
    }
        = string.Empty;

    public decimal QuantityUsed
    {
        get;
        set;
    }
}