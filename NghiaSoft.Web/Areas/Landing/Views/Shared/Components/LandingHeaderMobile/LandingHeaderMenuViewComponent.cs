using Microsoft.AspNetCore.Mvc;

namespace NghiaSoft.Web.Areas.Landing.Views.Shared.Components.LandingHeaderMobile;

public class LandingHeaderMobile : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}