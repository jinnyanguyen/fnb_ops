using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOps.Models;

/// <summary>
/// Tracks inventory changes for auditing.
/// </summary>
public class InventoryTransaction
{
    [Key]
    public int InventoryTransactionId { get; set; }

    public int IngredientId { get; set; }

    [ForeignKey(nameof(IngredientId))]
    public Ingredient? Ingredient { get; set; }

    /// <summary>
    /// Positive = Stock Added
    /// Negative = Stock Removed
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal QuantityChanged { get; set; }

    [Required]
    [StringLength(100)]
    public string Reason { get; set; } = string.Empty;

    public DateTime TransactionDate { get; set; }

    public int BranchId { get; set; }
}