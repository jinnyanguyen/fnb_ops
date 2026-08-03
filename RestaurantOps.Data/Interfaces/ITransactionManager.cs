namespace RestaurantOps.Data.Interfaces;

/// <summary>
/// Executes related database operations inside a single transaction.
/// If a transaction already exists, the current transaction is reused.
/// </summary>
public interface ITransactionManager
{
    /// <summary>
    /// Executes the supplied operation atomically.
    /// All database changes are committed together or rolled back together.
    /// </summary>
    /// <param name="operation">
    /// Database-related business operation to execute.
    /// </param>
    void Execute(Action operation);

    /// <summary>
    /// Executes an asynchronous operation inside a database transaction.
    /// Existing transactions are reused.
    /// </summary>
    Task ExecuteAsync(Func<Task> operation);
}