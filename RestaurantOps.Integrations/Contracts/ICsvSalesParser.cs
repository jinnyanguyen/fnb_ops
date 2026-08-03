using RestaurantOps.Models.Integrations;

namespace RestaurantOps.Integrations.Contracts;

/// <summary>
/// Converts a CSV stream into vendor-neutral sale commands.
///
/// This interface only parses and maps data. It does not save
/// entities, update inventory, or call Entity Framework.
/// </summary>
public interface ICsvSalesParser
{
    /// <summary>
    /// Reads sales records from a CSV stream.
    /// </summary>
    /// <param name="csvStream">
    /// Readable stream containing the CSV data.
    /// The caller remains responsible for disposing the stream.
    /// </param>
    /// <param name="cancellationToken">
    /// Allows parsing to stop when the request is cancelled
    /// or the application is shutting down.
    /// </param>
    /// <returns>
    /// Vendor-neutral sale commands ready for business validation.
    /// </returns>
    Task<IReadOnlyCollection<ImportedSaleCommand>> ParseAsync(
        Stream csvStream,
        CancellationToken cancellationToken = default);
}