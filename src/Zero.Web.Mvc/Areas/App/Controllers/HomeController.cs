using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class HomeController : ZeroControllerBase
    {
        public async Task<ActionResult> Index()
        {
            if (await IsGrantedAsync(AppPermissions.Dashboard))
                return RedirectToAction("Index", "Dashboard");
            //Default page if no permission to the pages above
            return RedirectToAction("Index", "Welcome");
        }
    }
}