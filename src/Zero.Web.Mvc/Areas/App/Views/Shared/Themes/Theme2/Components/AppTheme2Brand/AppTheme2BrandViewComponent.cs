using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zero.Web.Areas.App.Models.Layout;
using Zero.Web.Session;
using Zero.Web.Views;

namespace Zero.Web.Areas.App.Views.Shared.Themes.Theme2.Components.AppTheme2Brand
{
    public class AppTheme2BrandViewComponent : ZeroViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme2BrandViewComponent(IPerRequestSessionCache sessionCache)
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
