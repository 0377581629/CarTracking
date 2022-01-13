using System.IO;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Abp.Threading;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Zero;
using Zero.MultiTenancy;

namespace ZERO.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class FileManagerDataController : ContentProviderController
    {
        public FileManagerDataController(IMultiTenancyConfig multiTenancyConfig, IHostingEnvironment webHostEnvironment, IFileAppService fileAppService, IRepository<Tenant> tenantRepository)
            : base(webHostEnvironment, fileAppService, multiTenancyConfig, tenantRepository)
        {
        }

        public PartialViewResult FileManagerModal()
        {
            return PartialView("_FileManagerModal");
        }
    }
}