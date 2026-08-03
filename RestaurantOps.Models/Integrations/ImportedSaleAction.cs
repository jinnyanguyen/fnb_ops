namespace RestaurantOps.Models.Integrations;

/// <summary>
/// Describes how an externally supplied sale should be synchronized.
/// </summary>
public enum ImportedSaleAction
{
    Create = 0,
    Update = 1,
    Delete = 2
}