namespace RestaurantOps.Models;

/// <summary>
/// Describes an error encountered while processing
/// an individual POS import row.
/// </summary>
public class PosImportError
{
    /// <summary>
    /// CSV row number where the error occurred.
    /// The header is normally row 1, so data begins at row 2.
    /// </summary>
    public int RowNumber { get; set; }

    /// <summary>
    /// External product or recipe name read from the CSV file.
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// Human-readable explanation of why the row failed.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}