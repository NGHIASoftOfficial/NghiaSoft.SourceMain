using Microsoft.AspNetCore.Mvc;

namespace NghiaSoft.Web.Areas.Landing.Views.Shared.Components.LandingHeader;

public class LandingHeaderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}