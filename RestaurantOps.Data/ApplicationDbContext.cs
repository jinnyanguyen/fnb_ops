using Microsoft.EntityFrameworkCore;
using RestaurantOps.Models;
using RestaurantOps.Models.Integrations;

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

    /// <summary>
    /// Branch table
    /// </summary>
    public DbSet<Branch> Branches { get; set; }

    /// <summary>
    /// SOP template table.
    /// </summary>
    public DbSet<SOPTemplate> SOPTemplates { get; set; }

    /// <summary>
    /// SOP item table.
    /// </summary>
    public DbSet<SOPItem> SOPItems { get; set; }

    /// <summary>
    /// SOP execution table.
    /// </summary>
    public DbSet<SOPExecution> SOPExecutions { get; set; }

    /// <summary>
    /// SOP execution item table.
    /// </summary>
    public DbSet<SOPExecutionItem> SOPExecutionItems { get; set; }

    /// <summary>
    /// Recipe step table.
    /// </summary>
    public DbSet<RecipeStep> RecipeSteps { get; set; }

    /// <summary>
    /// Recipe execution table.
    /// </summary>
    public DbSet<RecipeExecution> RecipeExecutions { get; set; }

    /// <summary>
    /// Recipe execution step table.
    /// </summary>
    public DbSet<RecipeExecutionStep> RecipeExecutionSteps { get; set; }

    /// <summary>
    /// Inventory transaction table.
    /// Records all inventory movements for auditing and stock tracking.
    /// </summary>
    public DbSet<InventoryTransaction> InventoryTransactions { get; set; }

    /// <summary>
    /// External branch mapping table.
    /// Maps external POS store identifiers to internal Gusto Ops branches.
    /// </summary>
    public DbSet<ExternalBranchMapping> ExternalBranchMappings { get; set; }

    /// <summary>
    /// External recipe mapping table.
    /// Maps external POS menu item identifiers to internal Gusto Ops recipes.
    /// </summary>
    public DbSet<ExternalRecipeMapping> ExternalRecipeMappings { get; set; }

    /// <summary>
    /// Imported sale record table.
    /// Tracks successfully imported external sales to prevent duplicate processing.
    /// </summary>
    public DbSet<ImportedSaleRecord> ImportedSaleRecords { get; set; }

    /// <summary>
    /// Configures the Entity Framework Core model during application startup.
    /// This method defines database constraints and indexes that are not
    /// represented directly by entity classes.
    ///
    /// Unique indexes are configured to:
    /// - Ensure each external branch mapping is unique per source system.
    /// - Ensure each external recipe mapping is unique per source system.
    /// - Prevent duplicate processing of imported sales by enforcing a unique
    ///   combination of source system and external sale identifier.
    /// </summary>
    /// <param name="modelBuilder">
    /// Provides a fluent API for configuring entity mappings, indexes,
    /// constraints, and relationships.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Preserve the existing MySQL table name used by previous migrations.
        modelBuilder.Entity<Branch>()
            .ToTable("Branch");

        modelBuilder.Entity<ExternalBranchMapping>()
            .HasIndex(x => new { x.SourceSystem, x.ExternalStoreId })
            .IsUnique();

        modelBuilder.Entity<ExternalRecipeMapping>()
            .HasIndex(x => new { x.SourceSystem, x.ExternalItemId })
            .IsUnique();

        modelBuilder.Entity<ImportedSaleRecord>()
            .HasIndex(x => new { x.SourceSystem, x.ExternalSaleId })
            .IsUnique();
    }
}