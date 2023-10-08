using Microsoft.AspNetCore.Mvc;

namespace NghiaSoft.Web.Areas.Landing.Views.Shared.Components.LandingFooter;

public class LandingFooterViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}