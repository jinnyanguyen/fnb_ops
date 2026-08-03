using RestaurantOps.Data.Interfaces;

namespace RestaurantOps.Data.Transactions;

/// <summary>
/// Coordinates Entity Framework Core database transactions.
/// </summary>
public sealed class TransactionManager : ITransactionManager
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes the transaction manager with the shared application context.
    /// </summary>
    public TransactionManager(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public void Execute(Action operation)
    {
        ArgumentNullException.ThrowIfNull(operation);

        // Reuse an existing transaction when this method is called
        // from a larger workflow, such as a multi-item sale import.
        if (_context.Database.CurrentTransaction != null)
        {
            operation();
            return;
        }

        using var transaction =
            _context.Database.BeginTransaction();

        try
        {
            operation();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(Func<Task> operation)
    {
        ArgumentNullException.ThrowIfNull(operation);

        // Reuse an existing transaction when called by a larger workflow.
        if (_context.Database.CurrentTransaction != null)
        {
            await operation();
            return;
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            await operation();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}