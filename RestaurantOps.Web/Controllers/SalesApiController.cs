using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Models.Integrations;
using RestaurantOps.Web.ApiModels;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Provides REST API endpoints for external POS sales integration.
/// </summary>
[ApiController]
[Route("api/sales")]
public sealed class SalesApiController : ControllerBase
{
    private readonly IImportedSaleService _importedSaleService;
    private readonly ILogger<SalesApiController> _logger;

    /// <summary>
    /// Initializes the controller with its required dependencies.
    /// </summary>
    public SalesApiController(
        IImportedSaleService importedSaleService,
        ILogger<SalesApiController> logger)
    {
        _importedSaleService = importedSaleService;
        _logger = logger;
    }

    /// <summary>
    /// Imports an external sale through the shared Gusto Ops
    /// sales-processing workflow.
    /// </summary>
    /// <param name="request">
    /// Vendor-neutral external sale request.
    /// </param>
    /// <returns>
    /// Import status, created-sale count, and processing message.
    /// </returns>
    [HttpPost("import")]
    public async Task<IActionResult> ImportSale(
        [FromBody] ImportedSaleApiRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (!Enum.TryParse<ImportedSaleAction>(
                request.Action,
                ignoreCase: true,
                out var action))
        {
            ModelState.AddModelError(
                nameof(request.Action),
                "Action must be Create, Update, or Delete.");

            return ValidationProblem(ModelState);
        }

        var command = new ImportedSaleCommand
        {
            SourceSystem = request.SourceSystem,
            ExternalSaleId = request.ExternalSaleId,
            ExternalStoreId = request.ExternalStoreId,
            SaleDate = request.SaleDate,
            Action = action,

            Items = request.Items
                .Select(item => new ImportedSaleItemCommand
                {
                    ExternalItemId = item.ExternalItemId,
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    DiscountAmount = item.DiscountAmount,
                    ParentExternalItemId =
                        item.ParentExternalItemId,
                    IsKit = item.IsKit
                })
                .ToList()
        };

        try
        {
            var result =
                await _importedSaleService.ProcessAsync(command);

            return Ok(new
            {
                result.IsSuccessful,
                result.IsSkipped,
                result.SalesCreated,
                result.Message,
                request.SourceSystem,
                request.ExternalSaleId
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                ex,
                "External sale request failed validation.");

            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                ex,
                "External sale could not be processed.");

            return UnprocessableEntity(new
            {
                message = ex.Message
            });
        }
        catch (NotSupportedException ex)
        {
            _logger.LogWarning(
                ex,
                "Unsupported external sale operation.");

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = ex.Message
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while importing external sale.");

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "The external sale could not be imported."
                });
        }
    }
}