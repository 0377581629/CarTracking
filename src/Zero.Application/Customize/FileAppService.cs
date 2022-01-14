using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Zero.Authorization.Roles;
using Zero.Authorization.Users;

namespace Zero.Customize
{
    [AbpAuthorize]
    public class FileAppService : ZeroAppServiceBase, IFileAppService
    {
        #region Constructor

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly UserManager _userManager;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly RoleManager _roleManager;
        public FileAppService(
            IWebHostEnvironment webHostEnvironment, 
            IMultiTenancyConfig multiTenancyConfig, 
            UserManager userManager, 
            IRepository<UserRole, long> userRoleRepository, 
            RoleManager roleManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _multiTenancyConfig = multiTenancyConfig;
            _userManager = userManager;
            _userRoleRepository = userRoleRepository;
            _roleManager = roleManager;
        }

        #endregion

        public string PhysRootPath()
        {
            return _webHostEnvironment.WebRootPath;
        }

        public string FullFilePath(string input)
        {
            return _webHostEnvironment.WebRootFileProvider.GetFileInfo(input).PhysicalPath;
        }

        public async Task<string> FileFolder(string extentPath)
        {
            var targetPath = FileHelper.UploadPath(_multiTenancyConfig, AbpSession, await IsAdminUser());

            if (!string.IsNullOrEmpty(extentPath))
                targetPath = Path.Combine(targetPath, extentPath);
            
            var physicalPath = _webHostEnvironment.WebRootFileProvider.GetFileInfo(targetPath).PhysicalPath;

            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);
            return targetPath;
        }
        
        public async Task<string> RootFileServerBucketName()
        {
            return FileHelper.FileServerRootPath(_multiTenancyConfig, AbpSession, await IsAdminUser());
        }

        public async Task<string> SavePath(string extentPath = null)
        {
            return _webHostEnvironment.WebRootFileProvider.GetFileInfo(await FileFolder(extentPath)).PhysicalPath;
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
            var fileFolder = await FileFolder(null);
            var newFilePath = Path.Combine(_webHostEnvironment.WebRootFileProvider.GetFileInfo(fileFolder).PhysicalPath, newFileName);
            await File.WriteAllBytesAsync(newFilePath, data);
            return "/" + fileFolder.Replace(@"\", "/") + "/" + newFileName;
        }

        private async Task<bool> IsAdminUser()
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

            return isAdminUser;
        }
    }
}