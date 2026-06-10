using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Models;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles SOP template management.
/// Managers can create and manage SOP checklists.
/// </summary>
[Authorize(Roles = "Manager")]
public class SOPController : Controller
{
    private readonly ISOPService _sopService;
    private readonly ISOPExecutionService _executionService;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public SOPController(ISOPService sopService, ISOPExecutionService executionService)
    {
        _sopService = sopService;
        _executionService = executionService;
    }

    /// <summary>
    /// Displays SOP templates for the current branch.
    /// </summary>
    public IActionResult Index()
    {
        var branchIdClaim = User.FindFirst("BranchId")?.Value;

        if (string.IsNullOrEmpty(branchIdClaim))
        {
            return Unauthorized();
        }

        int branchId = int.Parse(branchIdClaim);

        var templates = _sopService.GetAll(branchId);

        return View(templates);
    }

    /// <summary>
    /// Displays create SOP form.
    /// </summary>
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Handles SOP template creation.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(SOPTemplate template)
    {
        if (!ModelState.IsValid)
        {
            return View(template);
        }

        // Automatically assign branch ownership
        template.BranchId = int.Parse(
            User.FindFirst("BranchId")!.Value);

        _sopService.Add(template);

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays SOP checklist item management page.
    /// </summary>
    public IActionResult Details(int id)
    {
        var sop = _sopService.GetById(id);

        if (sop == null)
        {
            return NotFound();
        }

        return View(sop);
    }

    /// <summary>
    /// Displays SOP execution history for managers.
    /// </summary>
    public IActionResult Executions()
    {
        var branchId = int.Parse(
            User.FindFirst("BranchId")!.Value);

        var executions =
            _executionService.GetExecutionsByBranch(branchId);

        return View(executions);
    }

    /// <summary>
    /// Adds a checklist item to an SOP template.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddItem(SOPItem item)
    {
        if (!ModelState.IsValid)
        {
            var sopReload = _sopService.GetById(item.SOPTemplateId);

            if (sopReload == null)
            {
                return NotFound();
            }

            return View("Details", sopReload);
        }

        _sopService.AddItem(item);

        return RedirectToAction(
            nameof(Details),
            new { id = item.SOPTemplateId });
    }


}