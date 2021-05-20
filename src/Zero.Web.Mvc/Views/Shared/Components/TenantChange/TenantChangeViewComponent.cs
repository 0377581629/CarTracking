using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zero.Web.Session;

namespace Zero.Web.Views.Shared.Components.TenantChange
{
    public class TenantChangeViewComponent : ZeroViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public TenantChangeViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loginInfo = await _sessionCache.GetCurrentLoginInformationsAsync();
            var model = ObjectMapper.Map<TenantChangeViewModel>(loginInfo);
            return View(model);
        }
    }
}
