using Microsoft.Extensions.Logging;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data.Repositories.Interfaces;
using RestaurantOps.Models.Integrations;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Provides business logic for duplicate detection
/// and recording external sale import outcomes.
/// </summary>
public class ImportedSaleRecordService
    : IImportedSaleRecordService
{
    private readonly IImportedSaleRecordRepository _repository;
    private readonly ILogger<ImportedSaleRecordService> _logger;

    /// <summary>
    /// Initializes the service with its required dependencies.
    /// </summary>
    public ImportedSaleRecordService(
        IImportedSaleRecordRepository repository,
        ILogger<ImportedSaleRecordService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<bool> HasBeenImportedAsync(
        string sourceSystem,
        string externalSaleId)
    {
        ValidateIdentifiers(sourceSystem, externalSaleId);

        return await _repository.ExistsSuccessfulAsync(
            sourceSystem.Trim(),
            externalSaleId.Trim());
    }

    /// <inheritdoc />
    public async Task RecordImportAsync(
        string sourceSystem,
        string externalSaleId,
        bool isSuccessful,
        string? message = null)
    {
        ValidateIdentifiers(sourceSystem, externalSaleId);

        var normalizedSource = sourceSystem.Trim();
        var normalizedSaleId = externalSaleId.Trim();

        var existingRecord =
            await _repository.GetByExternalIdAsync(
                normalizedSource,
                normalizedSaleId);

        if (existingRecord == null)
        {
            existingRecord = new ImportedSaleRecord
            {
                SourceSystem = normalizedSource,
                ExternalSaleId = normalizedSaleId,
                ImportedAtUtc = DateTime.UtcNow,
                IsSuccessful = isSuccessful,
                Message = message
            };

            _repository.Add(existingRecord);
        }
        else
        {
            existingRecord.ImportedAtUtc = DateTime.UtcNow;
            existingRecord.IsSuccessful = isSuccessful;
            existingRecord.Message = message;

            _repository.Update(existingRecord);
        }

        await _repository.SaveAsync();

        _logger.LogInformation(
            "External sale import result recorded. " +
            "Source: {SourceSystem}, ExternalSaleId: {ExternalSaleId}, " +
            "Success: {IsSuccessful}",
            normalizedSource,
            normalizedSaleId,
            isSuccessful);
    }

    /// <summary>
    /// Validates the external source and sale identifiers.
    /// </summary>
    private static void ValidateIdentifiers(
        string sourceSystem,
        string externalSaleId)
    {
        if (string.IsNullOrWhiteSpace(sourceSystem))
        {
            throw new ArgumentException(
                "Source system is required.",
                nameof(sourceSystem));
        }

        if (string.IsNullOrWhiteSpace(externalSaleId))
        {
            throw new ArgumentException(
                "External sale ID is required.",
                nameof(externalSaleId));
        }
    }
}