namespace RestaurantOps.Web.ViewModels;

/// <summary>
/// Top selling recipe analytics.
/// </summary>
public class TopSellingRecipeViewModel
{
    public string RecipeName
    {
        get;
        set;
    }
        = string.Empty;

    public int QuantitySold
    {
        get;
        set;
    }
}