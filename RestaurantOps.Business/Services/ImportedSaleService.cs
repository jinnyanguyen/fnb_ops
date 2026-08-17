using Microsoft.Extensions.Logging;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Business.Services.Interfaces;
using RestaurantOps.Data.Interfaces;
using RestaurantOps.Models;
using RestaurantOps.Models.Integrations;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Coordinates the complete external-sale import workflow.
/// </summary>
public sealed class ImportedSaleService : IImportedSaleService
{
    private readonly IExternalBranchMappingService _branchMappingService;
    private readonly IExternalRecipeMappingService _recipeMappingService;
    private readonly IImportedSaleRecordService _recordService;
    private readonly ISaleService _saleService;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<ImportedSaleService> _logger;

    /// <summary>
    /// Initializes the service with all required business dependencies.
    /// </summary>
    public ImportedSaleService(
        IExternalBranchMappingService branchMappingService,
        IExternalRecipeMappingService recipeMappingService,
        IImportedSaleRecordService recordService,
        ISaleService saleService,
        ITransactionManager transactionManager,
        ILogger<ImportedSaleService> logger)
    {
        _branchMappingService = branchMappingService;
        _recipeMappingService = recipeMappingService;
        _recordService = recordService;
        _saleService = saleService;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ImportedSaleResult> ImportAsync(
        ImportedSaleCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        ValidateCommand(command);

        var sourceSystem = command.SourceSystem.Trim();
        var externalSaleId = command.ExternalSaleId.Trim();

        if (await _recordService.HasBeenImportedAsync(
                sourceSystem,
                externalSaleId))
        {
            _logger.LogInformation(
                "External sale skipped because it was already imported. " +
                "Source: {SourceSystem}, ExternalSaleId: {ExternalSaleId}",
                sourceSystem,
                externalSaleId);

            return new ImportedSaleResult
            {
                IsSuccessful = true,
                IsSkipped = true,
                SalesCreated = 0,
                Message = "Sale was already imported successfully."
            };
        }

        if (command.Action != ImportedSaleAction.Create)
        {
            throw new NotSupportedException(
                $"Imported sale action '{command.Action}' is not supported yet.");
        }

        var salesCreated = 0;

        try
        {
            await _transactionManager.ExecuteAsync(async () =>
            {
                var branchId =
                    await _branchMappingService.GetBranchIdAsync(
                        sourceSystem,
                        command.ExternalStoreId.Trim());

                foreach (var item in command.Items)
                {
                    ValidateItem(item);

                    var recipeId =
                        await _recipeMappingService.GetRecipeIdAsync(
                            sourceSystem,
                            item.ExternalItemId.Trim());

                    var quantitySold =
                        decimal.ToInt32(item.Quantity);

                    var sale = new Sale
                    {
                        RecipeId = recipeId,
                        QuantitySold = quantitySold,
                        SaleDate = command.SaleDate.UtcDateTime,
                        BranchId = branchId
                    };

                    // Reuses existing sale, inventory-deduction,
                    // inventory-validation, and audit-log business logic.
                    _saleService.Add(sale);

                    salesCreated++;
                }

                await _recordService.RecordImportAsync(
                    sourceSystem,
                    externalSaleId,
                    true,
                    $"Imported {salesCreated} sale item(s).");
            });

            return new ImportedSaleResult
            {
                IsSuccessful = true,
                IsSkipped = false,
                SalesCreated = salesCreated,
                Message = $"Successfully imported {salesCreated} sale item(s)."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "External sale import failed. Source: {SourceSystem}, " +
                "ExternalSaleId: {ExternalSaleId}",
                sourceSystem,
                externalSaleId);

            // Record the failed attempt after the main transaction rolls back.
            await _recordService.RecordImportAsync(
                sourceSystem,
                externalSaleId,
                false,
                ex.Message);

            throw;
        }
    }

    /// <summary>
    /// Validates the imported sale header.
    /// </summary>
    private static void ValidateCommand(ImportedSaleCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.SourceSystem))
        {
            throw new ArgumentException(
                "Source system is required.",
                nameof(command));
        }

        if (string.IsNullOrWhiteSpace(command.ExternalSaleId))
        {
            throw new ArgumentException(
                "External sale ID is required.",
                nameof(command));
        }

        if (string.IsNullOrWhiteSpace(command.ExternalStoreId))
        {
            throw new ArgumentException(
                "External store ID is required.",
                nameof(command));
        }

        if (command.SaleDate == default)
        {
            throw new ArgumentException(
                "Sale date is required.",
                nameof(command));
        }

        if (command.Items.Count == 0)
        {
            throw new ArgumentException(
                "At least one sale item is required.",
                nameof(command));
        }
    }

    /// <summary>
    /// Validates one imported sale item.
    /// </summary>
    private static void ValidateItem(ImportedSaleItemCommand item)
    {
        if (string.IsNullOrWhiteSpace(item.ExternalItemId))
        {
            throw new ArgumentException(
                "External item ID is required.");
        }

        if (item.Quantity <= 0)
        {
            throw new ArgumentException(
                "Imported item quantity must be greater than zero.");
        }

        if (item.Quantity != decimal.Truncate(item.Quantity))
        {
            throw new ArgumentException(
                "Fractional quantities are not currently supported " +
                "by the internal Sale model.");
        }

        if (item.Quantity > int.MaxValue)
        {
            throw new ArgumentException(
                "Imported item quantity exceeds the supported limit.");
        }
    }
}