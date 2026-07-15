using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Defines operations used to import external
/// point-of-sale sales data into Gusto Ops.
/// </summary>
public interface IPosImportService
{
    /// <summary>
    /// Imports POS sales records from a CSV stream.
    /// </summary>
    /// <param name="csvStream">
    /// Stream containing the uploaded CSV data.
    /// </param>
    /// <param name="fileName">
    /// Original uploaded filename used for logging and validation.
    /// </param>
    /// <param name="branchId">
    /// Branch receiving the imported sales records.
    /// </param>
    /// <returns>
    /// A summary containing successful rows, failed rows,
    /// and detailed import errors.
    /// </returns>
    PosImportResult ImportSalesCsv(
        Stream csvStream,
        string fileName,
        int branchId);
}