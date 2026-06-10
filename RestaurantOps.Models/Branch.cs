using System.ComponentModel.DataAnnotations;

namespace RestaurantOps.Models;

/// <summary>
/// Represents a physical restaurant branch.
/// Most operational data in the system belongs to a branch,
/// allowing the platform to support multi-location management.
/// </summary>
public class Branch
{
    /// <summary>
    /// Primary key for the Branch table.
    /// </summary>
    public int BranchId { get; set; }

    /// <summary>
    /// Display name of the branch.
    /// Example: "Downtown", "Airport", "Mall Location".
    /// </summary>
    [Required(ErrorMessage = "Branch name is required.")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Physical location or address description for the branch.
    /// </summary>
    [StringLength(200)]
    public string? Location { get; set; }

    /// <summary>
    /// Date and time when the branch record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property for users assigned to this branch.
    /// </summary>
    public ICollection<User> Users { get; set; } = new List<User>();

    /// <summary>
    /// Navigation property for ingredients stocked by this branch.
    /// </summary>
    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    /// <summary>
    /// Navigation property for recipes used by this branch.
    /// </summary>
    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}