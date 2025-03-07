using Microsoft.AspNetCore.Mvc;

namespace FoodieR.Controllers;

public class FaqController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
