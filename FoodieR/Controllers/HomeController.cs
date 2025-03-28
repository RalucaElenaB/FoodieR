using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FoodieR.Models;

namespace FoodieR.Controllers;

public class HomeController : Controller
{
    //logger e din clasa utilitara Controller d easta nu e inregistrat ca dependinta/servciu in Program.cs
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
