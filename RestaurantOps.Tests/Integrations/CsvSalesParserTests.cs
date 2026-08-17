using System.Text;
using RestaurantOps.Integrations.Services;

namespace RestaurantOps.Tests.Integrations;

/// <summary>
/// Verifies CSV parsing, validation, grouping,
/// and conversion into ImportedSaleCommand objects.
/// </summary>
public class CsvSalesParserTests
{
    /// <summary>
    /// Creates an in-memory CSV stream for parser testing.
    /// </summary>
    private static MemoryStream CreateCsvStream(string csv)
    {
        return new MemoryStream(
            Encoding.UTF8.GetBytes(csv));
    }

    [Fact]
    public async Task ParseAsync_ValidSingleItemCsv_ReturnsOneSaleWithOneItem()
    {
        // Arrange
        const string csv = """
        SourceSystem,ExternalSaleId,ExternalStoreId,SaleDate,ExternalItemId,ItemName,Quantity,UnitPrice,DiscountAmount
        TEST-POS,CSV-SALE-001,HANOI-001,2026-08-14T10:00:00+07:00,PESTO-001,Hanoi Pesto,1,100000,0
        """;

        var parser = new CsvSalesParser();

        await using var stream =
            CreateCsvStream(csv);

        // Act
        var result =
            await parser.ParseAsync(stream);

        // Assert
        var command = Assert.Single(result);
        var item = Assert.Single(command.Items);

        Assert.Equal(
            "TEST-POS",
            command.SourceSystem);

        Assert.Equal(
            "CSV-SALE-001",
            command.ExternalSaleId);

        Assert.Equal(
            "HANOI-001",
            command.ExternalStoreId);

        Assert.Equal(
            "PESTO-001",
            item.ExternalItemId);

        Assert.Equal(
            1m,
            item.Quantity);
    }

    [Fact]
    public async Task ParseAsync_MultipleRowsSameSale_GroupsIntoOneCommand()
    {
        // Arrange
        const string csv = """
        SourceSystem,ExternalSaleId,ExternalStoreId,SaleDate,ExternalItemId,ItemName,Quantity,UnitPrice,DiscountAmount
        TEST-POS,CSV-SALE-002,HANOI-001,2026-08-14T10:00:00+07:00,PESTO-001,Hanoi Pesto,1,100000,0
        TEST-POS,CSV-SALE-002,HANOI-001,2026-08-14T10:00:00+07:00,DRINK-001,Lime Soda,2,30000,0
        """;

        var parser = new CsvSalesParser();

        await using var stream =
            CreateCsvStream(csv);

        // Act
        var result =
            await parser.ParseAsync(stream);

        // Assert
        var command = Assert.Single(result);

        Assert.Equal(
            2,
            command.Items.Count);
    }

    [Fact]
    public async Task ParseAsync_DifferentSaleIds_ReturnsMultipleCommands()
    {
        // Arrange
        const string csv = """
        SourceSystem,ExternalSaleId,ExternalStoreId,SaleDate,ExternalItemId,ItemName,Quantity,UnitPrice,DiscountAmount
        TEST-POS,CSV-SALE-003,HANOI-001,2026-08-14T10:00:00+07:00,PESTO-001,Hanoi Pesto,1,100000,0
        TEST-POS,CSV-SALE-004,HANOI-001,2026-08-14T11:00:00+07:00,PESTO-001,Hanoi Pesto,1,100000,0
        """;

        var parser = new CsvSalesParser();

        await using var stream =
            CreateCsvStream(csv);

        // Act
        var result =
            await parser.ParseAsync(stream);

        // Assert
        Assert.Equal(
            2,
            result.Count);
    }

    [Fact]
    public async Task ParseAsync_MissingRequiredHeader_ThrowsInvalidDataException()
    {
        // Arrange
        const string csv = """
        SourceSystem,ExternalSaleId,ExternalStoreId,SaleDate,ExternalItemId,ItemName,Quantity,UnitPrice
        TEST-POS,CSV-SALE-005,HANOI-001,2026-08-14T10:00:00+07:00,PESTO-001,Hanoi Pesto,1,100000
        """;

        var parser = new CsvSalesParser();

        await using var stream =
            CreateCsvStream(csv);

        // Act + Assert
        var exception =
            await Assert.ThrowsAsync<InvalidDataException>(
                () => parser.ParseAsync(stream));

        Assert.Contains(
            "DiscountAmount",
            exception.Message);
    }

    [Fact]
    public async Task ParseAsync_InvalidDate_ThrowsInvalidDataException()
    {
        // Arrange
        const string csv = """
        SourceSystem,ExternalSaleId,ExternalStoreId,SaleDate,ExternalItemId,ItemName,Quantity,UnitPrice,DiscountAmount
        TEST-POS,CSV-SALE-006,HANOI-001,NOT-A-DATE,PESTO-001,Hanoi Pesto,1,100000,0
        """;

        var parser = new CsvSalesParser();

        await using var stream =
            CreateCsvStream(csv);

        // Act + Assert
        await Assert.ThrowsAsync<InvalidDataException>(
            () => parser.ParseAsync(stream));
    }

    [Fact]
    public async Task ParseAsync_ZeroQuantity_ThrowsInvalidDataException()
    {
        // Arrange
        const string csv = """
        SourceSystem,ExternalSaleId,ExternalStoreId,SaleDate,ExternalItemId,ItemName,Quantity,UnitPrice,DiscountAmount
        TEST-POS,CSV-SALE-007,HANOI-001,2026-08-14T10:00:00+07:00,PESTO-001,Hanoi Pesto,0,100000,0
        """;

        var parser = new CsvSalesParser();

        await using var stream =
            CreateCsvStream(csv);

        // Act + Assert
        await Assert.ThrowsAsync<InvalidDataException>(
            () => parser.ParseAsync(stream));
    }

    [Fact]
    public async Task ParseAsync_SameSaleDifferentStores_ThrowsInvalidDataException()
    {
        // Arrange
        const string csv = """
        SourceSystem,ExternalSaleId,ExternalStoreId,SaleDate,ExternalItemId,ItemName,Quantity,UnitPrice,DiscountAmount
        TEST-POS,CSV-SALE-008,HANOI-001,2026-08-14T10:00:00+07:00,PESTO-001,Hanoi Pesto,1,100000,0
        TEST-POS,CSV-SALE-008,DANANG-001,2026-08-14T10:00:00+07:00,DRINK-001,Lime Soda,1,30000,0
        """;

        var parser = new CsvSalesParser();

        await using var stream =
            CreateCsvStream(csv);

        // Act + Assert
        await Assert.ThrowsAsync<InvalidDataException>(
            () => parser.ParseAsync(stream));
    }
}