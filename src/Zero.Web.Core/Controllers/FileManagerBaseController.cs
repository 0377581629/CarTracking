using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.Threading;
using Abp.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Z.EntityFramework.Extensions.Internal;
using Zero.Debugging;
using Zero.Web.FileManager.Interfaces;
using Zero.Web.FileManager.Model;

namespace Zero.Web.Controllers
{
    [DontWrapResult]
    public abstract class FileManagerBaseController : ZeroControllerBase, IFileManagerController
    {
        #region Constructor

        private readonly IContentBrowser _directoryBrowser;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IFileAppService _fileAppService;

        protected FileManagerBaseController(IContentBrowser directoryBrowser, IWebHostEnvironment hostingEnvironment, IFileAppService fileAppService)
        {
            _directoryBrowser = directoryBrowser;
            _directoryBrowser.HostingEnvironment = hostingEnvironment;
            _hostingEnvironment = hostingEnvironment;
            _fileAppService = fileAppService;
            _contentPath = RootContentPath();
        }

        #endregion

        private readonly string _contentPath;

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
                    var result = files.Concat(directories).Select(VirtualizePath).ToList();
                    return Json(result.ToArray());
                }
                catch (DirectoryNotFoundException)
                {
                    throw new Exception("File Not Found");
                }
            }

            var rootBucket = _contentPath;

            if (!string.IsNullOrEmpty(target))
            {
                target = target.Substring(target.IndexOf(rootBucket, StringComparison.InvariantCultureIgnoreCase) + rootBucket.Length + 1);
            }

            return Json((await GetFileEntryFromFileServer(rootBucket, target)).ToArray());
        }

        private async Task<List<FileManagerViewModel>> GetFileEntryFromFileServer(string path, string prefix)
        {
            var minioClient = new MinioClient(SystemConfig.MinioEndPoint, SystemConfig.MinioAccessKey, SystemConfig.MinioSecretKey);
            if (!DebugHelper.IsDebug)
                minioClient = minioClient.WithSSL();

            var rootBucket = _contentPath;

            if (!await minioClient.BucketExistsAsync(rootBucket))
                await minioClient.MakeBucketAsync(rootBucket);

            var lstFileEntry = new List<FileManagerViewModel>();
            var meth = minioClient.GetType().GetMethod("GetObjectListAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            if (meth == null) return lstFileEntry;
            // bucket name, prefix, delimiter - Empty if recursive = true
            var minioObjects = await minioClient.ListObjectsAsync(path, prefix: prefix, recursive: false);
            lstFileEntry = minioObjects
                .Select(item => new FileManagerViewModel
                {
                    Name = string.IsNullOrEmpty(prefix) ? item.Key.RemovePostFix("/") : item.Key.RemovePostFix("/").RemovePreFix(prefix),
                    Size = (long)item.Size,
                    Path = $"{(!DebugHelper.IsDebug ? "https" : "http")}://{SystemConfig.MinioEndPoint}/{SystemConfig.MinioRootBucketName}/{HttpUtility.UrlDecode(item.Key)}",
                    ActualPath = $"{(!DebugHelper.IsDebug ? "https" : "http")}://{SystemConfig.MinioEndPoint}/{SystemConfig.MinioRootBucketName}/{HttpUtility.UrlDecode(item.Key)}",
                    Extension = !item.IsDir ? Path.GetExtension(item.Key) : "",
                    IsDirectory = item.IsDir,
                    Modified = item.LastModifiedDateTime ?? DateTime.MinValue
                })
                .OrderBy(o => o.IsDirectory)
                .ToList();
            return lstFileEntry;
        }

        [DisableValidation]
        public ActionResult Create(string target, FileManagerViewModel viewModel)
        {
            FileManagerViewModel newViewModel;

            if (!Authorize(NormalizePath(target)))
            {
                throw new Exception("Forbidden");
            }


            if (String.IsNullOrEmpty(viewModel.Path))
            {
                newViewModel = CreateNewFolder(target, viewModel);
            }
            else
            {
                newViewModel = CopyEntry(target, viewModel);
            }

            return Json(VirtualizePath(newViewModel));
        }

        [DisableValidation]
        public ActionResult Destroy(FileManagerViewModel viewModel)
        {
            var path = NormalizePath(viewModel.Path);

            if (!string.IsNullOrEmpty(path))
            {
                if (viewModel.IsDirectory)
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
        public ActionResult Update(string target, FileManagerViewModel viewModel)
        {
            FileManagerViewModel newViewModel;

            if (!Authorize(NormalizePath(viewModel.Path)) && !Authorize(NormalizePath(target)))
            {
                throw new Exception("Forbidden");
            }

            newViewModel = RenameEntry(viewModel);

            return Json(VirtualizePath(newViewModel));
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

        private bool Authorize(string path)
        {
            return CanAccess(path);
        }

        private bool CanAccess(string path)
        {
            var rootPath = _hostingEnvironment.WebRootFileProvider.GetFileInfo(_contentPath).PhysicalPath;
            return path.StartsWith(rootPath);
        }

        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return _hostingEnvironment.WebRootFileProvider.GetFileInfo(_contentPath).PhysicalPath;
            }

            path = Path.Combine(_contentPath, path);
            
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

        private FileManagerViewModel VirtualizePath(FileManagerViewModel viewModel)
        {
            viewModel.Path = viewModel.Path
                .Replace(_hostingEnvironment.WebRootPath, "")
                //.Replace(@"wwwroot\", "")
                .Replace(_contentPath, "")
                .Replace(@"\", "/");
            viewModel.ActualPath = viewModel.ActualPath
                .Replace(_hostingEnvironment.WebRootPath, "")
                //.Replace(@"wwwroot\", "")
                // .Replace(_contentPath, "")
                .Replace(@"\", "/");
            if (viewModel.Path[0] == '/')
                viewModel.Path = viewModel.Path.Remove(0, 1);
            if (viewModel.ActualPath[0] == '/')
                viewModel.ActualPath = viewModel.ActualPath.Remove(0, 1);
            
            return viewModel;
        }

        private FileManagerViewModel CopyEntry(string target, FileManagerViewModel viewModel)
        {
            var path = NormalizePath(viewModel.Path);
            var physicalPath = path;
            var physicalTarget = EnsureUniqueName(NormalizePath(target), viewModel);

            FileManagerViewModel newViewModel;

            if (viewModel.IsDirectory)
            {
                CopyDirectory(new DirectoryInfo(physicalPath), Directory.CreateDirectory(physicalTarget));
                newViewModel = _directoryBrowser.GetDirectory(physicalTarget);
            }
            else
            {
                System.IO.File.Copy(physicalPath, physicalTarget);
                newViewModel = _directoryBrowser.GetFile(physicalTarget);
            }

            return newViewModel;
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

        private FileManagerViewModel CreateNewFolder(string target, FileManagerViewModel viewModel)
        {
            FileManagerViewModel newViewModel;
            var path = NormalizePath(target);
            var physicalPath = EnsureUniqueName(path, viewModel);

            Directory.CreateDirectory(physicalPath);

            newViewModel = _directoryBrowser.GetDirectory(physicalPath);

            return newViewModel;
        }

        private string EnsureUniqueName(string target, FileManagerViewModel viewModel)
        {
            var tempName = viewModel.Name + viewModel.Extension;
            var sequence = 0;
            var physicalTarget = Path.Combine(NormalizePath(target), tempName);

            if (viewModel.IsDirectory)
            {
                while (Directory.Exists(physicalTarget))
                {
                    tempName = viewModel.Name + String.Format("({0})", ++sequence);
                    physicalTarget = Path.Combine(NormalizePath(target), tempName);
                }
            }
            else
            {
                while (System.IO.File.Exists(physicalTarget))
                {
                    tempName = viewModel.Name + String.Format("({0})", ++sequence) + viewModel.Extension;
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

        private FileManagerViewModel RenameEntry(FileManagerViewModel viewModel)
        {
            var path = NormalizePath(viewModel.Path);
            var physicalPath = path;
            var physicalTarget = EnsureUniqueName(Path.GetDirectoryName(path), viewModel);
            FileManagerViewModel newViewModel;

            if (viewModel.IsDirectory)
            {
                Directory.Move(physicalPath, physicalTarget);
                newViewModel = _directoryBrowser.GetDirectory(physicalTarget);
            }
            else
            {
                var file = new FileInfo(physicalPath);
                System.IO.File.Move(file.FullName, physicalTarget);
                newViewModel = _directoryBrowser.GetFile(physicalTarget);
            }

            return newViewModel;
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
            if (fileContent.FileName != null) 
                return Path.GetFileName(fileContent.FileName.Trim('"'));
            throw new Exception("FileName is null");
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

        private string RootContentPath()
        {
            return SystemConfig.UseFileServer ? AsyncHelper.RunSync(() => _fileAppService.RootFileServerBucketName()) : AsyncHelper.RunSync(() => _fileAppService.FileFolder());
        }

        #endregion
    }
}