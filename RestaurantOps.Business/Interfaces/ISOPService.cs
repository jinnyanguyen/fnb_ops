using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

/// <summary>
/// Defines SOP business operations.
/// </summary>
public interface ISOPService
{
    /// <summary>
    /// Retrieves all SOP templates.
    /// </summary>
    List<SOPTemplate> GetAll();

    /// <summary>
    /// Retrieves SOP templates for a branch.
    /// </summary>
    List<SOPTemplate> GetAll(int branchId);

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
    /// Adds a checklist item to an SOP template.
    /// </summary>
    void AddItem(SOPItem item);

    /// <summary>
    /// Retrieves SOP items for a template.
    /// </summary>
    List<SOPItem> GetItems(int sopTemplateId);
}