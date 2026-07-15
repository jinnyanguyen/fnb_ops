using System.ComponentModel.DataAnnotations;

namespace RestaurantOps.Web.ApiModels;

/// <summary>
/// Request model used when importing sales
/// from an external POS system.
/// </summary>
public class PosSaleImportRequest
{
    [Required]
    public int RecipeId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int QuantitySold { get; set; }

    public DateTime? SaleDate { get; set; }

    [Required]
    public int BranchId { get; set; }

    public string? ExternalOrderId { get; set; }

    public string? SourceSystem { get; set; }
}