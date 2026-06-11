using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Web.Models;

namespace RestaurantOps.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Temporary shared error page.
    /// </summary>
    public IActionResult Error()
    {
        return View();
    }
}
