using Microsoft.EntityFrameworkCore;
using RestaurantOps.Data.Interfaces;
using RestaurantOps.Models;

namespace RestaurantOps.Data.Repositories;

/// <summary>
/// Handles SOP database operations.
/// Responsible only for data access.
/// </summary>
public class SOPRepository : ISOPRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public SOPRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all SOP templates.
    /// </summary>
    public List<SOPTemplate> GetAll()
    {
        return _context.SOPTemplates
            .Include(s => s.SOPItems)
            .ToList();
    }

    /// <summary>
    /// Retrieves SOP templates for a specific branch.
    /// </summary>
    public List<SOPTemplate> GetAllByBranch(int branchId)
    {
        return _context.SOPTemplates
            .Include(s => s.SOPItems)
            .Where(s => s.BranchId == branchId)
            .ToList();
    }

    /// <summary>
    /// Retrieves SOP template by ID.
    /// </summary>
    public SOPTemplate? GetById(int id)
    {
        return _context.SOPTemplates
            .Include(s => s.SOPItems)
            .FirstOrDefault(s => s.SOPTemplateId == id);
    }

    /// <summary>
    /// Adds a new SOP template.
    /// </summary>
    public void Add(SOPTemplate template)
    {
        _context.SOPTemplates.Add(template);
    }

    /// <summary>
    /// Updates an SOP template.
    /// </summary>
    public void Update(SOPTemplate template)
    {
        _context.SOPTemplates.Update(template);
    }

    /// <summary>
    /// Deletes an SOP template.
    /// </summary>
    public void Delete(int id)
    {
        var template = _context.SOPTemplates.Find(id);

        if (template != null)
        {
            _context.SOPTemplates.Remove(template);
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