using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Integrations.Contracts;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Provides API endpoints for importing external sales
/// from CSV files into the Gusto Ops sales workflow.
/// </summary>
[ApiController]
[Route("api/sales/csv")]
public sealed class SalesCsvImportController : ControllerBase
{
    private readonly ICsvSalesParser _csvSalesParser;
    private readonly IImportedSaleService _importedSaleService;
    private readonly ILogger<SalesCsvImportController> _logger;

    /// <summary>
    /// Initializes the CSV sales import controller.
    /// </summary>
    /// <param name="csvSalesParser">
    /// Parses uploaded CSV data into vendor-neutral
    /// imported sale commands.
    /// </param>
    /// <param name="importedSaleService">
    /// Processes imported sales through the existing
    /// Gusto Ops business workflow.
    /// </param>
    /// <param name="logger">
    /// Records CSV import activity and failures.
    /// </param>
    public SalesCsvImportController(
        ICsvSalesParser csvSalesParser,
        IImportedSaleService importedSaleService,
        ILogger<SalesCsvImportController> logger)
    {
        _csvSalesParser = csvSalesParser;
        _importedSaleService = importedSaleService;
        _logger = logger;
    }

    /// <summary>
    /// Imports sales from an uploaded CSV file.
    /// </summary>
    /// <param name="file">
    /// CSV file containing external sales records.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels processing if the HTTP request is aborted.
    /// </param>
    /// <returns>
    /// Summary of successfully processed, skipped,
    /// and failed external sales.
    /// </returns>
    [HttpPost("import")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ImportAsync(
        IFormFile? file,
        CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new
            {
                message = "A non-empty CSV file is required."
            });
        }

        var extension = Path.GetExtension(file.FileName);

        if (!string.Equals(
                extension,
                ".csv",
                StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new
            {
                message = "Only CSV files are supported."
            });
        }

        try
        {
            await using var stream = file.OpenReadStream();

            var commands = await _csvSalesParser.ParseAsync(
                stream,
                cancellationToken);

            var successful = 0;
            var skipped = 0;

            var results = new List<object>();

            foreach (var command in commands)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    var result =
                        await _importedSaleService.ImportAsync(
                            command);

                    if (result.IsSkipped)
                    {
                        skipped++;
                    }
                    else
                    {
                        successful++;
                    }

                    results.Add(new
                    {
                        externalSaleId =
                            command.ExternalSaleId,

                        success =
                            result.IsSuccessful,

                        skipped =
                            result.IsSkipped,

                        message =
                            result.Message
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "CSV sale import failed for external sale {ExternalSaleId}.",
                        command.ExternalSaleId);

                    results.Add(new
                    {
                        externalSaleId =
                            command.ExternalSaleId,

                        success = false,
                        skipped = false,
                        message = ex.Message
                    });
                }
            }

            var failed =
                results.Count - successful - skipped;

            _logger.LogInformation(
                "CSV import completed. File: {FileName}, Total: {Total}, Successful: {Successful}, Skipped: {Skipped}, Failed: {Failed}",
                file.FileName,
                results.Count,
                successful,
                skipped,
                failed);

            return Ok(new
            {
                message = "CSV import completed.",
                fileName = file.FileName,
                totalSales = results.Count,
                successful,
                skipped,
                failed,
                results
            });
        }
        catch (InvalidDataException ex)
        {
            _logger.LogWarning(
                ex,
                "CSV validation failed for file {FileName}.",
                file.FileName);

            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "CSV import was cancelled for file {FileName}.",
                file.FileName);

            return StatusCode(
                499,
                new
                {
                    message = "CSV import was cancelled."
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while importing CSV file {FileName}.",
                file.FileName);

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "An unexpected error occurred while importing the CSV file."
                });
        }
    }
}