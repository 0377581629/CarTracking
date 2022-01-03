using System.IO;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Zero.MultiTenancy;

namespace ZERO.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class FileManagerDataController : ContentProviderController
    {
        private readonly IRepository<Tenant> _tenantRepository;

        //
        // GET: /FileManager/
        private const string ContentFolderRoot = "Files";
                
        /// <summary>
        /// Gets the base paths from which content will be served.
        /// </summary>
        protected override string ContentPath => TenantFolder();

        public FileManagerDataController(IRepository<Tenant> tenantRepository, IHostingEnvironment webHostEnvironment)
           : base(webHostEnvironment)
        {
            _tenantRepository = tenantRepository;
        }

        /// <summary>
        /// Gets the valid file extensions by which served files will be filtered.
        /// </summary>
        protected override string Filter => "*.*";

        protected override bool CanAccess(string path)
        {
            return path.StartsWith(HostingEnvironment.WebRootFileProvider.GetFileInfo(TenantFolder()).PhysicalPath);
        }

        private string TenantFolder()
        {
            var targetPath = ContentFolderRoot;

            if (AbpSession.MultiTenancySide == MultiTenancySides.Tenant && AbpSession.TenantId.HasValue)
            {
                var currentTenant = _tenantRepository.Get(AbpSession.TenantId.Value);
                targetPath = Path.Combine(targetPath, currentTenant.TenancyName);
            } else {
                targetPath = Path.Combine(targetPath, "Host");
            }

            if (AbpSession.UserId.HasValue)
                targetPath = Path.Combine(targetPath, AbpSession.UserId.ToString());
            
            var physicalPath = HostingEnvironment.WebRootFileProvider.GetFileInfo(targetPath).PhysicalPath;

            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);
            return targetPath;
        }
        
        public PartialViewResult FileManagerModal()
        {
            return PartialView("_FileManagerModal");
        }
    }
}