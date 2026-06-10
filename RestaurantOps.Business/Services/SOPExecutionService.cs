using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data;
using RestaurantOps.Models;

namespace RestaurantOps.Business.Services;

/// <summary>
/// Handles SOP execution workflows.
/// Responsible for:
/// - starting execution sessions
/// - checklist completion tracking
/// - operational compliance logging
/// </summary>
public class SOPExecutionService : ISOPExecutionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SOPExecutionService> _logger;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public SOPExecutionService(
        ApplicationDbContext context,
        ILogger<SOPExecutionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Starts a new SOP execution session.
    /// </summary>
    public SOPExecution StartExecution(
        int sopTemplateId,
        int userId,
        int branchId)
    {
        _logger.LogInformation(
            "Starting SOP execution for template ID: {TemplateId}",
            sopTemplateId);

        var template = _context.SOPTemplates
            .Include(s => s.SOPItems)
            .FirstOrDefault(s => s.SOPTemplateId == sopTemplateId);

        if (template == null)
        {
            throw new Exception("SOP template not found.");
        }

        var execution = new SOPExecution
        {
            SOPTemplateId = sopTemplateId,
            UserId = userId,
            BranchId = branchId,
            ExecutedAt = DateTime.Now
        };

        // Create execution checklist items
        foreach (var item in template.SOPItems)
        {
            execution.ExecutionItems.Add(
                new SOPExecutionItem
                {
                    SOPItemId = item.SOPItemId,
                    IsCompleted = false
                });
        }

        _context.SOPExecutions.Add(execution);
        _context.SaveChanges();

        return execution;
    }

    /// <summary>
    /// Retrieves SOP execution session.
    /// </summary>
    public SOPExecution? GetExecution(int executionId)
    {
        return _context.SOPExecutions
            .Include(e => e.SOPTemplate)
            .Include(e => e.ExecutionItems)
                .ThenInclude(i => i.SOPItem)
            .FirstOrDefault(e =>
                e.SOPExecutionId == executionId);
    }

    /// <summary>
    /// Marks checklist item as completed.
    /// </summary>
    public void CompleteItem(int executionItemId)
    {
        var item = _context.SOPExecutionItems
            .FirstOrDefault(i =>
                i.SOPExecutionItemId == executionItemId);

        if (item == null)
        {
            throw new Exception("Execution item not found.");
        }

        item.IsCompleted = true;
        item.CompletedAt = DateTime.Now;

        _context.SaveChanges();

        _logger.LogInformation(
            "Completed SOP execution item ID: {ItemId}",
            executionItemId);
    }

    /// <summary>
    /// Retrieves SOP executions for a branch.
    /// </summary>
    public List<SOPExecution> GetExecutionsByBranch(int branchId)
    {
        return _context.SOPExecutions
            .Include(e => e.User)
            .Include(e => e.SOPTemplate)
            .Include(e => e.ExecutionItems)
            .Where(e => e.BranchId == branchId)
            .OrderByDescending(e => e.ExecutedAt)
            .ToList();
    }

    /// <summary>
    /// Retrieves SOP executions for a user.
    /// </summary>
    public List<SOPExecution> GetExecutionsByUser(int userId)
    {
        return _context.SOPExecutions
            .Include(e => e.SOPTemplate)
            .Include(e => e.ExecutionItems)
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.ExecutedAt)
            .ToList();
    }
}