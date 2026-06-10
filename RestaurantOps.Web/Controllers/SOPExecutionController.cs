using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles staff SOP execution workflow.
/// Staff can start SOPs and complete checklist items.
/// </summary>
[Authorize]
public class SOPExecutionController : Controller
{
    private readonly ISOPService _sopService;
    private readonly ISOPExecutionService _executionService;
    private readonly ApplicationDbContext _context;

    public SOPExecutionController(
        ISOPService sopService,
        ISOPExecutionService executionService,
        ApplicationDbContext context)
    {
        _sopService = sopService;
        _executionService = executionService;
        _context = context;
    }

    /// <summary>
    /// Shows SOP templates available to the logged-in user's branch.
    /// </summary>
    public IActionResult Index()
    {
        var branchId = int.Parse(User.FindFirst("BranchId")!.Value);

        var templates = _sopService.GetAll(branchId);

        return View(templates);
    }

    /// <summary>
    /// Displays execution history for the logged-in staff user.
    /// </summary>
    public IActionResult History()
    {
        var email = User.Identity?.Name;

        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized();
        }

        var user = _context.Users
            .FirstOrDefault(u => u.Email == email);

        if (user == null)
        {
            return Unauthorized();
        }

        var executions =
            _executionService.GetExecutionsByUser(user.UserId);

        return View(executions);
    }


    /// <summary>
    /// Starts an SOP execution session.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Start(int sopTemplateId)
    {
        var branchId = int.Parse(User.FindFirst("BranchId")!.Value);
        var email = User.Identity?.Name;

        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var user = _context.Users.FirstOrDefault(u => u.Email == email);

        if (user == null)
            return Unauthorized();

        var execution = _executionService.StartExecution(
            sopTemplateId,
            user.UserId,
            branchId);

        return RedirectToAction(
            nameof(Execute),
            new { id = execution.SOPExecutionId });
    }

    /// <summary>
    /// Displays checklist execution page.
    /// </summary>
    public IActionResult Execute(int id)
    {
        var execution = _executionService.GetExecution(id);

        if (execution == null)
            return NotFound();

        return View(execution);
    }

    /// <summary>
    /// Marks a checklist item as completed.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CompleteItem(int executionItemId, int executionId)
    {
        _executionService.CompleteItem(executionItemId);

        return RedirectToAction(
            nameof(Execute),
            new { id = executionId });
    }

    
}