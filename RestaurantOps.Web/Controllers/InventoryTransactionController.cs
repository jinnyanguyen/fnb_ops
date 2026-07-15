using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Business.Interfaces;

namespace RestaurantOps.Web.Controllers;

/// <summary>
/// Displays inventory audit history.
/// </summary>
[Authorize(Roles = "Manager")]
public class InventoryTransactionController
    : Controller
{
    private readonly
        IInventoryTransactionService
        _service;

    /// <summary>
    /// Constructor with dependency injection.
    /// </summary>
    public InventoryTransactionController(
        IInventoryTransactionService service)
    {
        _service = service;
    }

    /// <summary>
    /// Displays inventory history.
    /// </summary>
    public IActionResult Index()
    {
        var branchId =
            int.Parse(
                User.FindFirst("BranchId")!.Value);

        var transactions =
            _service.GetByBranch(
                branchId);

        return View(transactions);
    }
}