using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zero.Web.Areas.App.Models.Layout;
using Zero.Web.Session;
using Zero.Web.Views;

namespace Zero.Web.Areas.App.Views.Shared.Themes.Theme8.Components.AppTheme8Brand
{
    public class AppTheme8BrandViewComponent : ZeroViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme8BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
