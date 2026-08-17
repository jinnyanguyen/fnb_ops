using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using RestaurantOps.Integrations.Contracts;
using RestaurantOps.Models.Integrations;

namespace RestaurantOps.Integrations.Services;

/// <summary>
/// Reads external sales data from a CSV stream and converts
/// the records into vendor-neutral ImportedSaleCommand objects.
///
/// This parser is responsible only for CSV parsing, validation,
/// grouping, and mapping. It does not create sales, deduct inventory,
/// or access the database.
/// </summary>
public sealed class CsvSalesParser : ICsvSalesParser
{
    /// <summary>
    /// Required column names for the supported Gusto Ops
    /// vendor-neutral CSV format.
    /// </summary>
    private static readonly string[] RequiredHeaders =
    {
        "SourceSystem",
        "ExternalSaleId",
        "ExternalStoreId",
        "SaleDate",
        "ExternalItemId",
        "ItemName",
        "Quantity",
        "UnitPrice",
        "DiscountAmount"
    };

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<ImportedSaleCommand>> ParseAsync(
        Stream csvStream,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(csvStream);

        if (!csvStream.CanRead)
        {
            throw new ArgumentException(
                "The supplied CSV stream cannot be read.",
                nameof(csvStream));
        }

        using var reader = new StreamReader(
            csvStream,
            leaveOpen: true);

        var configuration = new CsvConfiguration(
            CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,

            // Header whitespace should not cause otherwise
            // valid files to fail unnecessarily.
            PrepareHeaderForMatch = args =>
                args.Header.Trim(),

            // We perform our own validation below so that
            // users receive clearer import-specific messages.
            MissingFieldFound = null,

            // CsvHelper will still throw for structurally
            // invalid CSV content.
            BadDataFound = context =>
            {
                throw new InvalidDataException(
                    $"Malformed CSV data detected near row " +
                    $"{context.Context.Parser.Row}.");
            }
        };

        using var csv = new CsvReader(
            reader,
            configuration);

        cancellationToken.ThrowIfCancellationRequested();

        // Ensure the file contains at least a header row.
        if (!await csv.ReadAsync())
        {
            throw new InvalidDataException(
                "The CSV file is empty.");
        }

        csv.ReadHeader();

        ValidateHeaders(csv.HeaderRecord);

        var records = new List<CsvSaleRecord>();
        var rowNumber = 1;

        while (await csv.ReadAsync())
        {
            cancellationToken.ThrowIfCancellationRequested();

            rowNumber++;

            var record = ParseRecord(
                csv,
                rowNumber);

            records.Add(record);
        }

        if (records.Count == 0)
        {
            throw new InvalidDataException(
                "The CSV file does not contain any sales records.");
        }

        return BuildCommands(records);
    }

    /// <summary>
    /// Ensures the CSV contains every column required
    /// by the supported import format.
    /// </summary>
    private static void ValidateHeaders(
        string[]? headers)
    {
        if (headers == null || headers.Length == 0)
        {
            throw new InvalidDataException(
                "The CSV file does not contain a valid header row.");
        }

        var normalizedHeaders = headers
            .Select(header => header.Trim())
            .ToHashSet(
                StringComparer.OrdinalIgnoreCase);

        var missingHeaders = RequiredHeaders
            .Where(required =>
                !normalizedHeaders.Contains(required))
            .ToList();

        if (missingHeaders.Count > 0)
        {
            throw new InvalidDataException(
                "The CSV file is missing required column(s): " +
                string.Join(", ", missingHeaders));
        }
    }

    /// <summary>
    /// Reads and validates one CSV row.
    /// </summary>
    private static CsvSaleRecord ParseRecord(
        CsvReader csv,
        int rowNumber)
    {
        var sourceSystem =
            GetRequiredField(
                csv,
                "SourceSystem",
                rowNumber);

        var externalSaleId =
            GetRequiredField(
                csv,
                "ExternalSaleId",
                rowNumber);

        var externalStoreId =
            GetRequiredField(
                csv,
                "ExternalStoreId",
                rowNumber);

        var saleDateText =
            GetRequiredField(
                csv,
                "SaleDate",
                rowNumber);

        var externalItemId =
            GetRequiredField(
                csv,
                "ExternalItemId",
                rowNumber);

        var itemName =
            GetRequiredField(
                csv,
                "ItemName",
                rowNumber);

        var quantityText =
            GetRequiredField(
                csv,
                "Quantity",
                rowNumber);

        var unitPriceText =
            GetRequiredField(
                csv,
                "UnitPrice",
                rowNumber);

        var discountText =
            GetRequiredField(
                csv,
                "DiscountAmount",
                rowNumber);

        if (!DateTimeOffset.TryParse(
                saleDateText,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind,
                out _))
        {
            throw new InvalidDataException(
                $"Row {rowNumber}: SaleDate " +
                $"'{saleDateText}' is not a valid date/time.");
        }

        if (!decimal.TryParse(
                quantityText,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var quantity) ||
            quantity <= 0)
        {
            throw new InvalidDataException(
                $"Row {rowNumber}: Quantity must be " +
                "a number greater than zero.");
        }

        if (quantity != decimal.Truncate(quantity))
        {
            throw new InvalidDataException(
                $"Row {rowNumber}: Fractional quantities are " +
                "not currently supported.");
        }

        if (!decimal.TryParse(
                unitPriceText,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var unitPrice) ||
            unitPrice < 0)
        {
            throw new InvalidDataException(
                $"Row {rowNumber}: UnitPrice must be " +
                "a valid non-negative number.");
        }

        if (!decimal.TryParse(
                discountText,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var discountAmount) ||
            discountAmount < 0)
        {
            throw new InvalidDataException(
                $"Row {rowNumber}: DiscountAmount must be " +
                "a valid non-negative number.");
        }

        return new CsvSaleRecord
        {
            SourceSystem = sourceSystem,
            ExternalSaleId = externalSaleId,
            ExternalStoreId = externalStoreId,
            SaleDate = saleDateText,
            ExternalItemId = externalItemId,
            ItemName = itemName,
            Quantity = quantity,
            UnitPrice = unitPrice,
            DiscountAmount = discountAmount
        };
    }

    /// <summary>
    /// Retrieves one required CSV field and produces
    /// a clear row-specific validation error when missing.
    /// </summary>
    private static string GetRequiredField(
        CsvReader csv,
        string columnName,
        int rowNumber)
    {
        var value = csv.GetField(columnName);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidDataException(
                $"Row {rowNumber}: Required field " +
                $"'{columnName}' is missing or empty.");
        }

        return value.Trim();
    }

    /// <summary>
    /// Groups CSV rows into external sales and maps them
    /// into the vendor-neutral import contract.
    /// </summary>
    private static IReadOnlyCollection<ImportedSaleCommand>
        BuildCommands(
            IReadOnlyCollection<CsvSaleRecord> records)
    {
        var commands = new List<ImportedSaleCommand>();

        var groups = records.GroupBy(record => new
        {
            SourceSystem =
                record.SourceSystem.Trim(),

            ExternalSaleId =
                record.ExternalSaleId.Trim()
        });

        foreach (var group in groups)
        {
            var first = group.First();

            ValidateGroupConsistency(group);

            var saleDate = DateTimeOffset.Parse(
                first.SaleDate,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind);

            var command = new ImportedSaleCommand
            {
                SourceSystem =
                    first.SourceSystem.Trim(),

                ExternalSaleId =
                    first.ExternalSaleId.Trim(),

                ExternalStoreId =
                    first.ExternalStoreId.Trim(),

                SaleDate = saleDate,

                Action =
                    ImportedSaleAction.Create,

                Items = group
                    .Select(record =>
                        new ImportedSaleItemCommand
                        {
                            ExternalItemId =
                                record.ExternalItemId.Trim(),

                            ItemName =
                                record.ItemName?.Trim(),

                            Quantity =
                                record.Quantity,

                            UnitPrice =
                                record.UnitPrice,

                            DiscountAmount =
                                record.DiscountAmount
                        })
                    .ToList()
            };

            commands.Add(command);
        }

        return commands;
    }

    /// <summary>
    /// Ensures rows belonging to the same external sale
    /// contain consistent sale-level information.
    /// </summary>
    private static void ValidateGroupConsistency(
        IEnumerable<CsvSaleRecord> group)
    {
        var records = group.ToList();
        var first = records[0];

        if (records.Any(record =>
                !string.Equals(
                    record.ExternalStoreId,
                    first.ExternalStoreId,
                    StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidDataException(
                $"External sale '{first.ExternalSaleId}' contains " +
                "multiple ExternalStoreId values.");
        }

        if (records.Any(record =>
                !string.Equals(
                    record.SaleDate,
                    first.SaleDate,
                    StringComparison.Ordinal)))
        {
            throw new InvalidDataException(
                $"External sale '{first.ExternalSaleId}' contains " +
                "multiple SaleDate values.");
        }
    }
}