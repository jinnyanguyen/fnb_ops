using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data;
using RestaurantOps.Models;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Handles business logic for sales.
/// </summary>
public class SaleService : ISaleService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SaleService> _logger;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public SaleService(ApplicationDbContext context, ILogger<SaleService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Returns all sales records, including related recipe data.
    /// </summary>
    public List<Sale> GetAll()
    {
        _logger.LogInformation("Fetching all sales records");

        return _context.Sales
            .Include(s => s.Recipe)
            .OrderByDescending(s => s.SaleDate)
            .ToList();
    }

    /// <summary>
    /// Adds a new sale record and calculates total amount automatically.
    /// </summary>
    public void Add(Sale sale)
    {
        var recipe = _context.Recipes.FirstOrDefault(r => r.RecipeId == sale.RecipeId);

        if (recipe == null)
        {
            throw new InvalidOperationException("Selected recipe does not exist.");
        }

        // AUTO SET DATE
        sale.SaleDate = DateTime.Now;

        // AUTO CALCULATE TOTAL
        sale.TotalAmount = sale.QuantitySold * recipe.SellingPrice;

        _logger.LogInformation(
            "Adding sale → Recipe: {RecipeId}, Qty: {Qty}, Total: {Total}",
            sale.RecipeId,
            sale.QuantitySold,
            sale.TotalAmount
        );

        _context.Sales.Add(sale);
        _context.SaveChanges();
    }
}