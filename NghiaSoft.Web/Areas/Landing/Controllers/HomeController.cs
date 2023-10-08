using Microsoft.AspNetCore.Mvc;

namespace NghiaSoft.Web.Areas.Landing.Controllers;


[Area("Landing")]
public class HomeController:Controller
{
    public IActionResult Index()
    {
        return View();
    }
}