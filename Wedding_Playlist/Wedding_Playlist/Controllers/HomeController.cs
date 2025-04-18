using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Wedding_Playlist.Models;
using Wedding_Playlist.Interfaces;

namespace Wedding_Playlist.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IDashboardService dashboardService) 
        : base(dashboardService)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.ShowDashboard = true;
        return View();
    }

    public IActionResult Privacy()
    {
        // Dashboard won't show on this page
        ViewBag.ShowDashboard = false;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
