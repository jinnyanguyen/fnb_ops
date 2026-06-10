using Microsoft.Extensions.Logging;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data.Interfaces;
using RestaurantOps.Models;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Handles SOP business logic.
/// Responsible for:
/// - SOP template management
/// - Branch-aware SOP filtering
/// - Operational workflow standardization
/// </summary>
public class SOPService : ISOPService
{
    private readonly ISOPRepository _repository;
    private readonly ILogger<SOPService> _logger;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public SOPService(
        ISOPRepository repository,
        ILogger<SOPService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all SOP templates.
    /// </summary>
    public List<SOPTemplate> GetAll()
    {
        _logger.LogInformation(
            "Fetching all SOP templates");

        return _repository.GetAll();
    }

    /// <summary>
    /// Retrieves SOP templates for a branch.
    /// </summary>
    public List<SOPTemplate> GetAll(int branchId)
    {
        _logger.LogInformation(
            "Fetching SOP templates for branch ID: {BranchId}",
            branchId);

        return _repository.GetAllByBranch(branchId);
    }

    /// <summary>
    /// Retrieves SOP template by ID.
    /// </summary>
    public SOPTemplate? GetById(int id)
    {
        _logger.LogInformation(
            "Fetching SOP template ID: {Id}",
            id);

        return _repository.GetById(id);
    }

    /// <summary>
    /// Adds a new SOP template.
    /// </summary>
    public void Add(SOPTemplate template)
    {
        _logger.LogInformation(
            "Creating SOP template: {Name}",
            template.Name);

        _repository.Add(template);
        _repository.Save();
    }

    /// <summary>
    /// Updates an SOP template.
    /// </summary>
    public void Update(SOPTemplate template)
    {
        _logger.LogInformation(
            "Updating SOP template ID: {Id}",
            template.SOPTemplateId);

        _repository.Update(template);
        _repository.Save();
    }

    /// <summary>
    /// Deletes an SOP template.
    /// </summary>
    public void Delete(int id)
    {
        _logger.LogWarning(
            "Deleting SOP template ID: {Id}",
            id);

        _repository.Delete(id);
        _repository.Save();
    }

    /// <summary>
/// Adds a checklist item to an SOP template.
/// </summary>
public void AddItem(SOPItem item)
{
    _logger.LogInformation(
        "Adding SOP item to template ID: {TemplateId}",
        item.SOPTemplateId);

    var template = _repository.GetById(item.SOPTemplateId);

    if (template == null)
    {
        throw new Exception("SOP template not found.");
    }

    template.SOPItems.Add(item);

    _repository.Update(template);
    _repository.Save();
}

/// <summary>
/// Retrieves checklist items for an SOP template.
/// </summary>
public List<SOPItem> GetItems(int sopTemplateId)
{
    var template = _repository.GetById(sopTemplateId);

    if (template == null)
    {
        return new List<SOPItem>();
    }

    return template.SOPItems
        .OrderBy(i => i.SortOrder)
        .ToList();
}
}