using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using Abp.Organizations;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization.Users;
using Zero.Authorization.Users.Dto;
using Zero.Customize;
using Zero.Dto;
using Zero.Dto.Tenancy;
using Zero.Editions;
using Zero.Editions.Dto;
using Zero.MultiTenancy;
using Zero.MultiTenancy.Dto;
using Zero.Organizations.Dto;

namespace Zero
{
    [AbpAuthorize]
    public class FileAppService : ZeroAppServiceBase, IFileAppService
    {
        #region Constructor

        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileAppService(
            IRepository<Tenant> tenantRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _tenantRepository = tenantRepository;
            _webHostEnvironment = webHostEnvironment;
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
    }
}