using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Handles live kitchen recipe execution.
/// </summary>
[Authorize]
public class RecipeExecutionController : Controller
{
    private readonly IRecipeService _recipeService;
    private readonly IRecipeExecutionService _executionService;
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public RecipeExecutionController(
        IRecipeService recipeService,
        IRecipeExecutionService executionService,
        ApplicationDbContext context)
    {
        _recipeService = recipeService;
        _executionService = executionService;
        _context = context;
    }

    /// <summary>
    /// Displays recipes available for execution.
    /// </summary>
    public IActionResult Index()
    {
        var branchId = int.Parse(
            User.FindFirst("BranchId")!.Value);

        var recipes =
            _recipeService.GetAll(branchId);

        return View(recipes);
    }

    /// <summary>
    /// Starts recipe execution session.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Start(int recipeId)
    {
        var branchId = int.Parse(
            User.FindFirst("BranchId")!.Value);

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

        var execution =
            _executionService.StartExecution(
                recipeId,
                user.UserId,
                branchId);

        return RedirectToAction(
            nameof(Execute),
            new { id = execution.RecipeExecutionId });
    }

    /// <summary>
    /// Displays recipe execution workflow.
    /// </summary>
    public IActionResult Execute(int id)
    {
        var execution =
            _executionService.GetExecution(id);

        if (execution == null)
        {
            return NotFound();
        }

        return View(execution);
    }

    /// <summary>
    /// Completes recipe execution step.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CompleteStep(
        int executionStepId,
        int executionId)
    {
        _executionService.CompleteStep(
            executionStepId);

        return RedirectToAction(
            nameof(Execute),
            new { id = executionId });
    }

    /// <summary>
    /// Displays user's recipe execution history.
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
            _executionService
                .GetExecutionsByUser(user.UserId);

        return View(executions);
    }

    /// <summary>
    /// Displays branch kitchen monitoring dashboard.
    /// </summary>
    [Authorize(Roles = "Manager")]
    public IActionResult Monitoring()
    {
        var branchId = int.Parse(
            User.FindFirst("BranchId")!.Value);

        var executions =
            _executionService
                .GetExecutionsByBranch(branchId);

        return View(executions);
    }
}