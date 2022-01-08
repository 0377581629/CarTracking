using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Zero.Authorization.Roles;
using Zero.Authorization.Users;
using Zero.MultiTenancy;

namespace Zero.Customize
{
    [AbpAuthorize]
    public class FileAppService : ZeroAppServiceBase, IFileAppService
    {
        #region Constructor

        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly UserManager _userManager;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly RoleManager _roleManager;
        public FileAppService(
            IRepository<Tenant> tenantRepository,
            IWebHostEnvironment webHostEnvironment, 
            IMultiTenancyConfig multiTenancyConfig, 
            UserManager userManager, 
            IRepository<UserRole, long> userRoleRepository, 
            RoleManager roleManager)
        {
            _tenantRepository = tenantRepository;
            _webHostEnvironment = webHostEnvironment;
            _multiTenancyConfig = multiTenancyConfig;
            _userManager = userManager;
            _userRoleRepository = userRoleRepository;
            _roleManager = roleManager;
        }

        #endregion

        private const string ContentFolderRoot = "Files";

        public string PhysRootPath()
        {
            return _webHostEnvironment.WebRootPath;
        }

        public string FullFilePath(string input)
        {
            return _webHostEnvironment.WebRootFileProvider.GetFileInfo(input).PhysicalPath;
        }

        public string FileFolder(string extentPath)
        {
            var targetPath = ContentFolderRoot;

            if (AbpSession.MultiTenancySide == MultiTenancySides.Tenant && AbpSession.TenantId.HasValue)
            {
                var currentTenant = _tenantRepository.Get(AbpSession.TenantId.Value);
                targetPath = Path.Combine(targetPath, currentTenant.TenancyName);
            }
            else
            {
                targetPath = Path.Combine(targetPath, "Host");
            }

            if (AbpSession.UserId.HasValue)
                targetPath = Path.Combine(targetPath, AbpSession.UserId.ToString());

            if (!string.IsNullOrEmpty(extentPath))
                targetPath = Path.Combine(targetPath, extentPath);
            
            
            var physicalPath = _webHostEnvironment.WebRootFileProvider.GetFileInfo(targetPath).PhysicalPath;

            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);
            return targetPath;
        }

        public string SavePath(string extentPath = null)
        {
            return _webHostEnvironment.WebRootFileProvider.GetFileInfo(FileFolder(extentPath)).PhysicalPath;
        }

        public string PhysicalPath(string input)
        {
            var fullPath = _webHostEnvironment.WebRootFileProvider.GetFileInfo(input).PhysicalPath;
            return !File.Exists(fullPath) ? "" : fullPath;
        }
        
        public string RelativePath(string absolutePath)
        {
            return absolutePath.RemovePreFix(PhysRootPath()).Replace(@"\","/");
        }
        
        public void Copy(string fromPath, string toPath)
        {
            try
            {
                if (File.Exists(fromPath) && !File.Exists(toPath))
                    File.Copy(fromPath, toPath);
            }
            catch (Exception)
            {
                Logger.Error($"Cannot copy File {fromPath} TO {toPath}");
            }
        }

        public string NewFileName(string fileName)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}-{StringHelper.ShortIdentity(10)}{Path.GetExtension(fileName)}";
        }

        public string NewGuidFileName(string fileName)
        {
            return $"a{Guid.NewGuid().ToString().Replace("-","")}{Path.GetExtension(fileName)}";
        }

        public async Task<string> SaveFile(string newFileName, byte[] data)
        {
            var fileFolder = FileFolder(null);
            var newFilePath = Path.Combine(_webHostEnvironment.WebRootFileProvider.GetFileInfo(fileFolder).PhysicalPath, newFileName);
            await File.WriteAllBytesAsync(newFilePath, data);
            return "/" + fileFolder.Replace(@"\", "/") + "/" + newFileName;
        }

        public async Task<string> RootFileServerBucketName()
        {
            var currentUser = await _userManager.GetUserAsync(AbpSession.ToUserIdentifier());
            var roleIds = await  _userRoleRepository.GetAll()
                .Where(o=>o.UserId == currentUser.Id)
                .Select(o=>o.RoleId).ToListAsync();
                
            var isAdminUser = false;
            if (roleIds.Any())
            {
                var roles = await _roleManager.Roles.Where(x => roleIds.Contains(x.Id)).ToListAsync();
                isAdminUser = roles.FirstOrDefault(o => o.Name == StaticRoleNames.Tenants.Admin) != null;
            }
            return FileHelper.FileServerRootPath(_multiTenancyConfig, AbpSession, isAdminUser);
        }
    }
}