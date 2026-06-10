using RestaurantOps.Models;

namespace RestaurantOps.Data.Interfaces;

/// <summary>
/// Repository interface for SOP data access.
/// Responsible only for database operations.
/// </summary>
public interface ISOPRepository
{
    /// <summary>
    /// Retrieves all SOP templates.
    /// </summary>
    List<SOPTemplate> GetAll();

    /// <summary>
    /// Retrieves SOP templates for a specific branch.
    /// </summary>
    List<SOPTemplate> GetAllByBranch(int branchId);

    /// <summary>
    /// Retrieves SOP template by ID.
    /// </summary>
    SOPTemplate? GetById(int id);

    /// <summary>
    /// Adds a new SOP template.
    /// </summary>
    void Add(SOPTemplate template);

    /// <summary>
    /// Updates an SOP template.
    /// </summary>
    void Update(SOPTemplate template);

    /// <summary>
    /// Deletes an SOP template.
    /// </summary>
    void Delete(int id);

    /// <summary>
    /// Saves database changes.
    /// </summary>
    void Save();
}