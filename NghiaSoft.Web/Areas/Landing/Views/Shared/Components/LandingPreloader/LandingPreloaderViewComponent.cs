using Microsoft.AspNetCore.Mvc;

namespace NghiaSoft.Web.Areas.Landing.Views.Shared.Components.LandingPreloader;

public class LandingPreloaderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke() => View();
}