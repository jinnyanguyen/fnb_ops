using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Models;
using RestaurantOps.Web.ApiModels;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// API controller used for external POS sales integration.
/// </summary>
[ApiController]
[Route("api/sales")]
public class SalesApiController : ControllerBase
{
    private readonly ISaleService _saleService;
    private readonly ILogger<SalesApiController> _logger;

    public SalesApiController(
        ISaleService saleService,
        ILogger<SalesApiController> logger)
    {
        _saleService = saleService;
        _logger = logger;
    }

    /// <summary>
    /// Imports a sale from an external POS system.
    /// </summary>
    [HttpPost("import")]
    public IActionResult ImportSale(
        [FromBody] PosSaleImportRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var sale = new Sale
            {
                RecipeId = request.RecipeId,
                QuantitySold = request.QuantitySold,
                SaleDate = request.SaleDate ?? DateTime.Now,
                BranchId = request.BranchId
            };

            _saleService.Add(sale);

            _logger.LogInformation(
                "POS sale imported successfully. Source: {SourceSystem}, ExternalOrderId: {ExternalOrderId}",
                request.SourceSystem,
                request.ExternalOrderId);

            return Ok(new
            {
                message = "Sale imported successfully.",
                recipeId = sale.RecipeId,
                quantitySold = sale.QuantitySold,
                saleDate = sale.SaleDate,
                branchId = sale.BranchId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to import POS sale.");

            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}