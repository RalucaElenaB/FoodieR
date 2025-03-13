using Microsoft.AspNetCore.Mvc;

namespace FoodieR.Controllers;

public class AboutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
