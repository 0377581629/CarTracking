using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Zero.Web.Controllers;
using Zero.Web.FileManager.Interfaces;

namespace Zero.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class FilesManagerController : FileManagerBaseController
    {
        public FilesManagerController(IContentBrowser contentBrowser, IWebHostEnvironment webHostEnvironment, IFileAppService fileAppService)
            : base(contentBrowser, webHostEnvironment, fileAppService)
        {
        }
        
        public ActionResult Index()
        {
            return View();
        }
        
        public PartialViewResult FileManagerModal()
        {
            return PartialView("_FileManagerModal");
        }
    }
}