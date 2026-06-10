using Microsoft.EntityFrameworkCore;
using RestaurantOps.Data.Interfaces;
using RestaurantOps.Models;

namespace RestaurantOps.Data.Repositories;

/// <summary>
/// Handles Sale database operations.
/// Responsible only for data access.
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public SaleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all sales.
    /// </summary>
    public List<Sale> GetAll()
    {
        return _context.Sales
            .Include(s => s.Recipe)
            .ToList();
    }

    /// <summary>
    /// Retrieves sales for a specific branch.
    /// </summary>
    public List<Sale> GetAllByBranch(int branchId)
    {
        return _context.Sales
            .Include(s => s.Recipe)
            .Where(s => s.BranchId == branchId)
            .ToList();
    }

    /// <summary>
    /// Retrieves sale by ID.
    /// </summary>
    public Sale? GetById(int id)
    {
        return _context.Sales
            .Include(s => s.Recipe)
            .FirstOrDefault(s => s.SaleId == id);
    }

    /// <summary>
    /// Adds a new sale.
    /// </summary>
    public void Add(Sale sale)
    {
        _context.Sales.Add(sale);
    }

    /// <summary>
    /// Updates an existing sale.
    /// </summary>
    public void Update(Sale sale)
    {
        _context.Sales.Update(sale);
    }

    /// <summary>
    /// Deletes sale by ID.
    /// </summary>
    public void Delete(int id)
    {
        var sale = _context.Sales.Find(id);

        if (sale != null)
        {
            _context.Sales.Remove(sale);
        }
    }

    /// <summary>
    /// Saves database changes.
    /// </summary>
    public void Save()
    {
        _context.SaveChanges();
    }
}