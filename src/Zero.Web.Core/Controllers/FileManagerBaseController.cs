using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.Threading;
using Abp.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.Exceptions;
using Z.EntityFramework.Extensions.Internal;
using Zero.Customize;
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
        
        private const string TemporaryDirectoryFileName = "A8A36F9090B59479A855524D18B00743C.txt";
        private const string TemporaryDirectoryFilePath = "/assets/SampleFiles/A8A36F9090B59479A855524D18B00743C.txt";
            
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

            if (string.IsNullOrEmpty(target))
                return Json((await FileServerGetObjects(target)).ToArray());
            if (target.EndsWith("/"))
                target = target.RemovePostFix("/");
            var allObjects = await FileServerGetObjects(target);
            
            return Json(allObjects.Where(o=>!o.ActualPath.EndsWith(TemporaryDirectoryFileName)).ToArray());
        }

        [DisableValidation]
        public async Task<ActionResult> Create(string target, FileManagerViewModel viewModel)
        {
            if (!SystemConfig.UseFileServer)
            {
                // Copy Entry call when file manager enable drag feature
                if (!Authorize(NormalizePath(target))) throw new Exception("Forbidden");
                var newViewModel = string.IsNullOrEmpty(viewModel.Path) ? CreateNewFolder(target, viewModel) : CopyEntry(target, viewModel);
                return Json(VirtualizePath(newViewModel));    
            }

            viewModel.Name = await FileServerEnsureUniqueFolderName(target, viewModel);
            
            var path = $"{Path.Combine(string.IsNullOrEmpty(target) ? "" : target, viewModel.Name, TemporaryDirectoryFileName).Replace(@"\", "/")}";
            var folderPath = $"{Path.Combine(string.IsNullOrEmpty(target) ? "" : target, viewModel.Name).Replace(@"\", "/")}";
            var temporaryDirectoryPhysPath = _hostingEnvironment.WebRootFileProvider.GetFileInfo(TemporaryDirectoryFilePath).PhysicalPath;
            await using var fs = System.IO.File.Open(temporaryDirectoryPhysPath, FileMode.Open, FileAccess.Read);
            
            await FileServerUpload(path, fs.Length, fs);
            
            return Json(new FileManagerViewModel
            {
                Name = Path.GetFileNameWithoutExtension(viewModel.Name),
                Path = folderPath,
                ActualPath = $"{(!DebugHelper.IsDebug ? "https" : "http")}://{SystemConfig.MinioEndPoint}/{SystemConfig.MinioRootBucketName}/{folderPath}",
                Size = fs.Length,
                Extension = "",
                Modified = DateTime.Now,
                ModifiedUtc = DateTime.UtcNow,
                IsDirectory = true
            });
        }

        [DisableValidation]
        public async Task<ActionResult> Destroy(FileManagerViewModel viewModel)
        {
            if (!SystemConfig.UseFileServer)
            {
                var path = NormalizePath(viewModel.Path);

                if (string.IsNullOrEmpty(path)) throw new Exception("File Not Found");

                if (viewModel.IsDirectory)
                {
                    DeleteDirectory(path);
                }
                else
                {
                    DeleteFile(path);
                }
            }

            string oldKey;
            if (viewModel.IsDirectory)
            {
                oldKey = viewModel.ActualPath.RemovePreFix($"{(!DebugHelper.IsDebug ? "https" : "http")}://{SystemConfig.MinioEndPoint}/{SystemConfig.MinioRootBucketName}/");
                var lstToMove = await FileServerGetObjects(oldKey, true);
                await FileServerDeleteObjects(lstToMove.Select(o=>o.Path).ToList());
            }
            else
            {
                oldKey = viewModel.ActualPath.RemovePreFix($"{(!DebugHelper.IsDebug ? "https" : "http")}://{SystemConfig.MinioEndPoint}/{SystemConfig.MinioRootBucketName}/");
                await FileServerDeleteObject(oldKey);
            }

            return Json(Array.Empty<object>());
        }

        [DisableValidation]
        public async Task<ActionResult> Update(string target, FileManagerViewModel viewModel)
        {
            if (!SystemConfig.UseFileServer)
            {
                if (!Authorize(NormalizePath(viewModel.Path)) && !Authorize(NormalizePath(target)))
                {
                    throw new Exception("Forbidden");
                }

                var newViewModel = RenameEntry(viewModel);

                return Json(VirtualizePath(newViewModel));
            }

            string oldKey, newKey;
            if (viewModel.IsDirectory)
            {
                oldKey = viewModel.ActualPath.RemovePreFix($"{(!DebugHelper.IsDebug ? "https" : "http")}://{SystemConfig.MinioEndPoint}/{SystemConfig.MinioRootBucketName}");
                // newKey = oldKey.RemovePostFix(Path.GetFileName(viewModel.Path) + "/") + $"{viewModel.Name}/";
                var lstToMove = await FileServerGetObjects(oldKey, true);
                oldKey = oldKey.RemovePreFix("/");
                newKey = oldKey.RemovePostFix(Path.GetFileName(viewModel.Path) + "/") + $"{viewModel.Name}/";
                foreach (var obj in lstToMove)
                {
                    var newObjKey = newKey + obj.Path.RemovePreFix(oldKey);
                    await FileServerCopyObject(obj.Path, newObjKey);
                }
                await FileServerDeleteObjects(lstToMove.Select(o=>o.Path).ToList());
                
                return Json(new FileManagerViewModel
                {
                    Name = viewModel.Name,
                    Path = newKey.RemovePostFix("/"),
                    ActualPath = $"{(!DebugHelper.IsDebug ? "https" : "http")}://{SystemConfig.MinioEndPoint}/{SystemConfig.MinioRootBucketName}/{newKey}",
                    Size = viewModel.Size,
                    Extension = "",
                    IsDirectory = viewModel.IsDirectory,
                    HasDirectories = viewModel.HasDirectories,
                    Modified = DateTime.Now,
                    ModifiedUtc = DateTime.UtcNow
                });
            }
            
            oldKey = viewModel.ActualPath.RemovePreFix($"{(!DebugHelper.IsDebug ? "https" : "http")}://{SystemConfig.MinioEndPoint}/{SystemConfig.MinioRootBucketName}/");
            newKey = oldKey.RemovePostFix(Path.GetFileName(viewModel.Path)) + $"{viewModel.Name}{viewModel.Extension}";
            await FileServerCopyObject(oldKey, newKey);
            await FileServerDeleteObject(oldKey);
            return Json(new FileManagerViewModel
            {
                Name = Path.GetFileNameWithoutExtension(viewModel.Name),
                Path = newKey,
                ActualPath = $"{(!DebugHelper.IsDebug ? "https" : "http")}://{SystemConfig.MinioEndPoint}/{SystemConfig.MinioRootBucketName}/{newKey}",
                Size = viewModel.Size,
                Extension = viewModel.Extension,
                Modified = DateTime.Now,
                ModifiedUtc = DateTime.UtcNow,
                IsDirectory = viewModel.IsDirectory
            });
            
        }

        [AcceptVerbs("POST")]
        [DisableValidation]
        public async Task<ActionResult> Upload(string path, IFormFile file)
        {
            if (!SystemConfig.UseFileServer)
            {
                path = NormalizePath(path);
                var fileName = Path.GetFileName(file.FileName);

                if (!AuthorizeUpload(path, file))
                    throw new Exception("Forbidden");
                SaveFile(file, path);
                var newEntry = _directoryBrowser.GetFile(Path.Combine(path, fileName));
                return Json(VirtualizePath(newEntry));
            }

            FileHelper.Validate(file);

            path = $"{Path.Combine(string.IsNullOrEmpty(path) ? _contentPath : path, file.FileName).Replace(@"\", "/")}";

            await FileServerUpload(path, file.Length, file.OpenReadStream());

            return Json(new FileManagerViewModel
            {
                Name = Path.GetFileNameWithoutExtension(file.FileName),
                Path = path,
                ActualPath = $"{(!DebugHelper.IsDebug ? "https" : "http")}://{SystemConfig.MinioEndPoint}/{SystemConfig.MinioRootBucketName}/{path}",
                Size = file.Length,
                Extension = Path.GetExtension(file.FileName),
                Modified = DateTime.Now,
                ModifiedUtc = DateTime.UtcNow,
                IsDirectory = false
            });
        }

        #region Support Method - Normal

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
                .RemovePreFix(_hostingEnvironment.WebRootPath + _contentPath)
                .Replace(@"\", "/");
            viewModel.ActualPath = viewModel.ActualPath
                .RemovePreFix(_hostingEnvironment.WebRootPath)
                .Replace(@"\", "/");
            if (viewModel.IsDirectory)
            {
                if (viewModel.Path[0] == '/')
                    viewModel.Path = viewModel.Path.Remove(0, 1);
                if (viewModel.ActualPath[0] == '/')
                    viewModel.ActualPath = viewModel.ActualPath.Remove(0, 1);
            }
            else
            {
                if (viewModel.Path[0] != '/')
                    viewModel.Path = $"/{viewModel.Path}";
                if (viewModel.ActualPath[0] != '/')
                    viewModel.ActualPath = $"/{viewModel.ActualPath}";
            }

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
            var path = NormalizePath(target);
            var physicalPath = EnsureUniqueName(path, viewModel);

            Directory.CreateDirectory(physicalPath);

            var newViewModel = _directoryBrowser.GetDirectory(physicalPath);

            return VirtualizePath(newViewModel);
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
                    tempName = viewModel.Name + $"({++sequence})";
                    physicalTarget = Path.Combine(NormalizePath(target), tempName);
                }
            }
            else
            {
                while (System.IO.File.Exists(physicalTarget))
                {
                    tempName = viewModel.Name + $"({++sequence})" + viewModel.Extension;
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
            var physicalTarget = EnsureUniqueName(Path.GetDirectoryName(path), viewModel);
            FileManagerViewModel newViewModel;

            if (viewModel.IsDirectory)
            {
                Directory.Move(path, physicalTarget);
                newViewModel = _directoryBrowser.GetDirectory(physicalTarget);
            }
            else
            {
                var file = new FileInfo(path);
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

        #region Support Method - File Server

        private async Task<List<FileManagerViewModel>> FileServerGetObjects(string prefix, bool recursive = false)
        {
            var minioClient = new MinioClient(SystemConfig.MinioEndPoint, SystemConfig.MinioAccessKey, SystemConfig.MinioSecretKey);
            if (!DebugHelper.IsDebug)
                minioClient = minioClient.WithSSL();

            if (!await minioClient.BucketExistsAsync(_contentPath))
                await minioClient.MakeBucketAsync(_contentPath);

            if (!string.IsNullOrEmpty(prefix) && !prefix.EndsWith("/"))
                prefix += "/";
            // bucket name, prefix, delimiter - Empty if recursive = true
            var minioObjects = await minioClient.ListObjectsAsync(_contentPath, prefix, recursive);
            var lstFileEntry = minioObjects
                .Select(item => new FileManagerViewModel
                {
                    Name = Path.GetFileNameWithoutExtension(!string.IsNullOrEmpty(prefix) ? item.Key.RemovePostFix("/").RemovePreFix(prefix) : item.Key.RemovePostFix("/")),
                    Size = (long)item.Size,
                    Path = item.Key.RemovePostFix("/"),
                    ActualPath = $"{(!DebugHelper.IsDebug ? "https" : "http")}://{SystemConfig.MinioEndPoint}/{SystemConfig.MinioRootBucketName}/{item.Key}",
                    Extension = !item.IsDir ? Path.GetExtension(item.Key) : "",
                    IsDirectory = item.IsDir,
                    Modified = item.LastModifiedDateTime ?? DateTime.MinValue
                })
                .OrderByDescending(o => o.IsDirectory)
                .ToList();
            return lstFileEntry;
        }

        private async Task<string> FileServerEnsureUniqueFolderName(string target, FileManagerViewModel viewModel)
        {
            var folderName = viewModel.Name;
            var path = $"{Path.Combine(string.IsNullOrEmpty(target) ? "" : target, folderName, TemporaryDirectoryFileName).Replace(@"\", "/")}";
            
            var sequence = 0;
            
            while (await FileServerCheckExisted(path) && sequence <= 2000)
            {
                folderName = viewModel.Name + $"({++sequence})";
                path = $"{Path.Combine(string.IsNullOrEmpty(target) ? "" : target, folderName, TemporaryDirectoryFileName).Replace(@"\", "/")}";
            }
            
            return folderName;
        }
        
        private async Task<bool> FileServerCheckExisted(string objectKey)
        {
            var minioClient = new MinioClient(SystemConfig.MinioEndPoint, SystemConfig.MinioAccessKey, SystemConfig.MinioSecretKey);
            if (!DebugHelper.IsDebug)
                minioClient = minioClient.WithSSL();

            if (!await minioClient.BucketExistsAsync(_contentPath))
                await minioClient.MakeBucketAsync(_contentPath);
            try
            {
                await minioClient.StatObjectAsync(_contentPath, objectKey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task FileServerUpload(string objectKey, long size, Stream file)
        {
            try
            {
                var minioClient = new MinioClient(SystemConfig.MinioEndPoint, SystemConfig.MinioAccessKey, SystemConfig.MinioSecretKey);
                if (!DebugHelper.IsDebug)
                    minioClient = minioClient.WithSSL();

                var rootBucket = _contentPath;

                if (!await minioClient.BucketExistsAsync(rootBucket))
                    await minioClient.MakeBucketAsync(rootBucket);

                // Upload a file to bucket.
                await minioClient.PutObjectAsync(rootBucket, objectKey, size, file);
            }
            catch (MinioException e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
            }
        }

        private async Task FileServerCopyObject(string objectKey, string desObjectKey)
        {
            if (objectKey == desObjectKey)
                return;
            try
            {
                var minioClient = new MinioClient(SystemConfig.MinioEndPoint, SystemConfig.MinioAccessKey, SystemConfig.MinioSecretKey);
                if (!DebugHelper.IsDebug)
                    minioClient = minioClient.WithSSL();

                var rootBucket = _contentPath;

                if (!await minioClient.BucketExistsAsync(rootBucket))
                    await minioClient.MakeBucketAsync(rootBucket);

                await minioClient.CopyObjectAsync(rootBucket, objectKey, rootBucket, desObjectKey);
            }
            catch (MinioException e)
            {
                Console.WriteLine("File Copy Error: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("File Copy Error: {0}", e.Message);
            }
        }

        private async Task FileServerDeleteObject(string objectKey)
        {
            try
            {
                var minioClient = new MinioClient(SystemConfig.MinioEndPoint, SystemConfig.MinioAccessKey, SystemConfig.MinioSecretKey);
                if (!DebugHelper.IsDebug)
                    minioClient = minioClient.WithSSL();

                if (!await minioClient.BucketExistsAsync(_contentPath))
                    await minioClient.MakeBucketAsync(_contentPath);

                await minioClient.RemoveObjectAsync(_contentPath, objectKey);
            }
            catch (MinioException e)
            {
                Console.WriteLine("Remove Error: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Remove Error: {0}", e.Message);
            }
        }

        private async Task FileServerDeleteObjects(List<string> objectKeys)
        {
            try
            {
                var minioClient = new MinioClient(SystemConfig.MinioEndPoint, SystemConfig.MinioAccessKey, SystemConfig.MinioSecretKey);
                if (!DebugHelper.IsDebug)
                    minioClient = minioClient.WithSSL();

                var rootBucket = _contentPath;

                if (!await minioClient.BucketExistsAsync(rootBucket))
                    await minioClient.MakeBucketAsync(rootBucket);

                // Upload a file to bucket.
                var observable  = await minioClient.RemoveObjectAsync(rootBucket, objectKeys);
                observable.Wait();
            }
            catch (MinioException e)
            {
                Console.WriteLine("Remove Error: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Remove Error: {0}", e.Message);
            }
        }
        
        #endregion
    }
}