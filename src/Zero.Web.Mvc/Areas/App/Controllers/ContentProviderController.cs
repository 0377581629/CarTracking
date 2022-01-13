using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.Json;
using Abp.Runtime.Validation;
using Abp.Threading;
using Abp.Web.Models;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel;
using Z.EntityFramework.Extensions.Internal;
using Zero;
using Zero.Debugging;
using Zero.MultiTenancy;
using Zero.Web.Controllers;

namespace ZERO.Web.Areas.App.Controllers
{
    [DontWrapResult]
    public abstract class ContentProviderController : ZeroControllerBase, IContentProviderController
    {
        #region Constructor

        private readonly IContentBrowser _directoryBrowser;
#pragma warning disable CS0618
        private readonly IHostingEnvironment _hostingEnvironment;
#pragma warning restore CS0618
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IFileAppService _fileAppService;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private MinioClient _minioClient = new(SystemConfig.MinioEndPoint, SystemConfig.MinioAccessKey, SystemConfig.MinioSecretKey);

        protected ContentProviderController(IHostingEnvironment hostingEnvironment, IFileAppService fileAppService, IMultiTenancyConfig multiTenancyConfig, IRepository<Tenant> tenantRepository)
            : this(DI.Current.Resolve<IContentBrowser>(),
                hostingEnvironment, fileAppService, multiTenancyConfig, tenantRepository)
        {
        }

        protected ContentProviderController(IContentBrowser directoryBrowser,
            IHostingEnvironment hostingEnvironment, IFileAppService fileAppService, IMultiTenancyConfig multiTenancyConfig, IRepository<Tenant> tenantRepository)
        {
            _directoryBrowser = directoryBrowser;
            _directoryBrowser.HostingEnvironment = hostingEnvironment;
            _hostingEnvironment = hostingEnvironment;
            _fileAppService = fileAppService;
            _multiTenancyConfig = multiTenancyConfig;
            _tenantRepository = tenantRepository;
            ContentPath = RootContentPath();
        }

        #endregion

        private readonly string ContentPath;

        private static string Filter => "*.*";

        public async Task<JsonResult> Read(string target, string filter)
        {
            if (!SystemConfig.UseFileServer)
            {
                var path = NormalizePath(target);

                if (!Authorize(path)) throw new Exception("Forbidden");

                try
                {
                    var files = _directoryBrowser.GetFiles(path, !string.IsNullOrEmpty(filter) ? filter : Filter);
                    var directories = _directoryBrowser.GetDirectories(path);
                    var result = files.Concat(directories).Select(VirtualizePath);

                    return Json(result.ToArray());
                }
                catch (DirectoryNotFoundException)
                {
                    throw new Exception("File Not Found");
                }
            }

            if (!DebugHelper.IsDebug)
                _minioClient = _minioClient.WithSSL();

            var rootBucket = ContentPath;

            if (!await _minioClient.BucketExistsAsync(rootBucket))
                await _minioClient.MakeBucketAsync(rootBucket);

            if (!string.IsNullOrEmpty(target))
            {
                target = target.Substring(target.IndexOf(rootBucket, StringComparison.InvariantCultureIgnoreCase) + rootBucket.Length + 1);
            }
            
            return Json((await GetFileEntryFromFileServer(rootBucket,target)).ToArray());
        }

        private async Task<List<FileManagerEntry>> GetFileEntryFromFileServer(string path, string prefix)
        {
            var lstFileEntry = new List<FileManagerEntry>();
            var meth = _minioClient.GetType().GetMethod("GetObjectListAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            if (meth == null) return lstFileEntry;
            // bucket name, prefix, delimiter - Empty if recursive = true
            var minioObjects = await _minioClient.ListObjectsAsync(path, prefix: prefix, recursive: false);
            lstFileEntry = minioObjects
                .Select( item => new FileManagerEntry
                {
                    Name = HttpUtility.UrlDecode(item.Key),
                    Size = (long)item.Size,
                    Path = $"{(!DebugHelper.IsDebug ? "https" : "http")}://{SystemConfig.MinioEndPoint}/{SystemConfig.MinioRootBucketName}/{HttpUtility.UrlDecode(item.Key)}",
                    Extension = !item.IsDir ? Path.GetExtension(item.Key) : "",
                    IsDirectory = item.IsDir,
                    Modified = item.LastModifiedDateTime ?? DateTime.MinValue
                })
                .OrderBy(o=>o.IsDirectory)
                .ToList();
            return lstFileEntry;
        }

        [DisableValidation]
        public ActionResult Create(string target, FileManagerEntry entry)
        {
            FileManagerEntry newEntry;

            if (!Authorize(NormalizePath(target)))
            {
                throw new Exception("Forbidden");
            }


            if (String.IsNullOrEmpty(entry.Path))
            {
                newEntry = CreateNewFolder(target, entry);
            }
            else
            {
                newEntry = CopyEntry(target, entry);
            }

            return Json(VirtualizePath(newEntry));
        }

        [DisableValidation]
        public ActionResult Destroy(FileManagerEntry entry)
        {
            var path = NormalizePath(entry.Path);

            if (!string.IsNullOrEmpty(path))
            {
                if (entry.IsDirectory)
                {
                    DeleteDirectory(path);
                }
                else
                {
                    DeleteFile(path);
                }

                return Json(new object[0]);
            }

            throw new Exception("File Not Found");
        }

        [DisableValidation]
        public ActionResult Update(string target, FileManagerEntry entry)
        {
            FileManagerEntry newEntry;

            if (!Authorize(NormalizePath(entry.Path)) && !Authorize(NormalizePath(target)))
            {
                throw new Exception("Forbidden");
            }

            newEntry = RenameEntry(entry);

            return Json(VirtualizePath(newEntry));
        }

        [AcceptVerbs("POST")]
        [DisableValidation]
        public virtual ActionResult Upload(string path, IFormFile file)
        {
            path = NormalizePath(path);
            var fileName = Path.GetFileName(file.FileName);

            if (!AuthorizeUpload(path, file)) throw new Exception("Forbidden");

            SaveFile(file, path);
            var newEntry = _directoryBrowser.GetFile(Path.Combine(path, fileName));

            return Json(VirtualizePath(newEntry));
        }

        #region Support Method

        private bool CanAccess(string path)
        {
            return path.StartsWith(_hostingEnvironment.WebRootFileProvider.GetFileInfo(RootContentPath()).PhysicalPath);
        }

        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return Path.GetFullPath(Path.Combine(_hostingEnvironment.WebRootPath, ContentPath));
            }

            path = path.Replace("/", @"\");
            var pathElements = path.Split(@"\").ToList();
            pathElements = pathElements.Where(o => !string.IsNullOrEmpty(o)).ToList();
            path = _hostingEnvironment.WebRootPath;
            foreach (var pathElement in pathElements)
            {
                path += @"\" + pathElement;
            }

            if (path.Contains(_hostingEnvironment.WebRootPath))
            {
                while (path.Contains(_hostingEnvironment.WebRootPath))
                {
                    path = path.ReplaceFirst(_hostingEnvironment.WebRootPath, "");
                }

                path = _hostingEnvironment.WebRootPath + path;
            }

            return Path.GetFullPath(path);
        }

        private bool Authorize(string path)
        {
            return CanAccess(path);
        }

        private FileManagerEntry VirtualizePath(FileManagerEntry entry)
        {
            entry.Path = entry.Path.Replace(Path.Combine(_hostingEnvironment.WebRootPath), "").Replace(@"\", "/");
            if (entry.Path[0] == '/')
                entry.Path = entry.Path.Remove(0, 1);
            return entry;
        }

        private FileManagerEntry CopyEntry(string target, FileManagerEntry entry)
        {
            var path = NormalizePath(entry.Path);
            var physicalPath = path;
            var physicalTarget = EnsureUniqueName(NormalizePath(target), entry);

            FileManagerEntry newEntry;

            if (entry.IsDirectory)
            {
                CopyDirectory(new DirectoryInfo(physicalPath), Directory.CreateDirectory(physicalTarget));
                newEntry = _directoryBrowser.GetDirectory(physicalTarget);
            }
            else
            {
                System.IO.File.Copy(physicalPath, physicalTarget);
                newEntry = _directoryBrowser.GetFile(physicalTarget);
            }

            return newEntry;
        }

        private void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (var fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (var diSourceSubDir in source.GetDirectories())
            {
                var nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyDirectory(diSourceSubDir, nextTargetSubDir);
            }
        }

        private FileManagerEntry CreateNewFolder(string target, FileManagerEntry entry)
        {
            FileManagerEntry newEntry;
            var path = NormalizePath(target);
            var physicalPath = EnsureUniqueName(path, entry);

            Directory.CreateDirectory(physicalPath);

            newEntry = _directoryBrowser.GetDirectory(physicalPath);

            return newEntry;
        }

        private string EnsureUniqueName(string target, FileManagerEntry entry)
        {
            var tempName = entry.Name + entry.Extension;
            var sequence = 0;
            var physicalTarget = Path.Combine(NormalizePath(target), tempName);

            if (entry.IsDirectory)
            {
                while (Directory.Exists(physicalTarget))
                {
                    tempName = entry.Name + String.Format("({0})", ++sequence);
                    physicalTarget = Path.Combine(NormalizePath(target), tempName);
                }
            }
            else
            {
                while (System.IO.File.Exists(physicalTarget))
                {
                    tempName = entry.Name + String.Format("({0})", ++sequence) + entry.Extension;
                    physicalTarget = Path.Combine(NormalizePath(target), tempName);
                }
            }

            return physicalTarget;
        }

        private void DeleteFile(string path)
        {
            if (!Authorize(path))
            {
                throw new Exception("Forbidden");
            }

            var physicalPath = NormalizePath(path);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }

        private void DeleteDirectory(string path)
        {
            if (!Authorize(path))
            {
                throw new Exception("Forbidden");
            }

            var physicalPath = NormalizePath(path);

            if (Directory.Exists(physicalPath))
            {
                Directory.Delete(physicalPath, true);
            }
        }

        private bool IsValidFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var allowedExtensions = Filter.Split(',');

            return allowedExtensions.Any(e => e.Equals("*.*") || e.EndsWith(extension, StringComparison.OrdinalIgnoreCase));
        }

        private FileManagerEntry RenameEntry(FileManagerEntry entry)
        {
            var path = NormalizePath(entry.Path);
            var physicalPath = path;
            var physicalTarget = EnsureUniqueName(Path.GetDirectoryName(path), entry);
            FileManagerEntry newEntry;

            if (entry.IsDirectory)
            {
                Directory.Move(physicalPath, physicalTarget);
                newEntry = _directoryBrowser.GetDirectory(physicalTarget);
            }
            else
            {
                var file = new FileInfo(physicalPath);
                System.IO.File.Move(file.FullName, physicalTarget);
                newEntry = _directoryBrowser.GetFile(physicalTarget);
            }

            return newEntry;
        }

        private bool AuthorizeUpload(string path, IFormFile file)
        {
            if (!CanAccess(path))
            {
                throw new DirectoryNotFoundException($"The specified path cannot be found - {path}");
            }

            if (!IsValidFile(GetFileName(file)))
            {
                throw new InvalidDataException(
                    $"The type of file is not allowed. Only {Filter} extensions are allowed.");
            }

            return true;
        }

        private string GetFileName(IFormFile file)
        {
            var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            return Path.GetFileName(fileContent.FileName.Trim('"'));
        }

        private void SaveFile(IFormFile file, string pathToSave)
        {
            try
            {
                var path = Path.Combine(pathToSave, GetFileName(file));
                using var stream = System.IO.File.Create(path);
                file.CopyTo(stream);
            }
            catch (Exception ex)
            {
                Logger.Error("Upload - SaveFile", ex);
                throw new Exception(ex.Message);
            }
        }

        private const string ContentFolderRoot = "Files";

        private string RootContentPath()
        {
            if (SystemConfig.UseFileServer) return AsyncHelper.RunSync(() => _fileAppService.RootFileServerBucketName());

            var targetPath = ContentFolderRoot;

            if (_multiTenancyConfig.IsEnabled && AbpSession.TenantId.HasValue)
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

            var physicalPath = _hostingEnvironment.WebRootFileProvider.GetFileInfo(targetPath).PhysicalPath;

            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);

            return targetPath;
        }

        #endregion
    }

    public interface IContentProviderController
    {
        Task<JsonResult> Read(string target, string filter);

        ActionResult Destroy(FileManagerEntry entry);

        ActionResult Create(string target, FileManagerEntry entry);

        ActionResult Update(string target, FileManagerEntry entry);

        ActionResult Upload(string path, IFormFile file);
    }
}