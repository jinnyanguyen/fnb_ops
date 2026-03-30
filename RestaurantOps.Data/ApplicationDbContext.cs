using Microsoft.EntityFrameworkCore;
using RestaurantOps.Models;

namespace RestaurantOps.Data;

/// <summary>
/// Main database context that connects Entity Framework to MySQL.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Constructor required for Dependency Injection.
    /// </summary>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Ingredients table
    /// </summary>
    public DbSet<Ingredient> Ingredients { get; set; }

    /// <summary>
    /// Recipes table
    /// </summary>
    public DbSet<Recipe> Recipes { get; set; }

    /// <summary>
    /// Sales table
    /// </summary>
    public DbSet<Sale> Sales { get; set; }

    /// <summary>
    /// Users table
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Recipe Ingredient table
    /// </summary>
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

/// <summary>
/// Task Assignment table
/// </summary>
    public DbSet<TaskAssignment> TaskAssignments { get; set; }
}