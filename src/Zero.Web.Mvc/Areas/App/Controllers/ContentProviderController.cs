#if NETSTANDARD1_6
    using Microsoft.Net.Http.Headers;
#else
using System.Net.Http.Headers;
#endif
using System;
using System.IO;
using System.Linq;
using Abp.Runtime.Validation;
using Abp.UI;
using Abp.Web.Models;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayPalHttp;
using Z.EntityFramework.Extensions.Internal;
using Zero.Web.Controllers;

namespace ZERO.Web.Areas.App.Controllers
{
    [DontWrapResult]
    public abstract class ContentProviderController : ZeroControllerBase, IContentProviderController
    {
        private readonly IContentBrowser _directoryBrowser;
        private readonly IContentPermission _permission;
        protected readonly IHostingEnvironment HostingEnvironment;

        protected ContentProviderController(IHostingEnvironment hostingEnvironment)
            : this(DI.Current.Resolve<IContentBrowser>(),
                   DI.Current.Resolve<IContentPermission>(),
                   hostingEnvironment)
        {
        }

        protected ContentProviderController(IContentBrowser directoryBrowser,
            IContentPermission permission,
            IHostingEnvironment hostingEnvironment)
        {
            _directoryBrowser = directoryBrowser;
            _directoryBrowser.HostingEnvironment = hostingEnvironment;
            _permission = permission;
            HostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Gets the base path from which content will be served.
        /// </summary>
        protected abstract string ContentPath
        {
            get;
        }

        /// <summary>
        /// Gets the valid file extensions by which served files will be filtered.
        /// </summary>
        protected virtual string Filter => "*.*";

        /// <summary>
        /// Determines if content of a given path can be browsed.
        /// </summary>
        /// <param name="path">The path which will be browsed.</param>
        /// <returns>true if browsing is allowed, otherwise false.</returns>
        public virtual bool Authorize(string path)
        {
            return CanAccess(path);
        }

        protected virtual bool CanAccess(string path)
        {
            var rootPath = Path.GetFullPath(Path.Combine(HostingEnvironment.WebRootPath, ContentPath));

            return _permission.CanAccess(rootPath, path);
        }

        protected string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return Path.GetFullPath(Path.Combine(HostingEnvironment.WebRootPath, ContentPath));
            }
            path = path.Replace("/",@"\");
            var pathElements = path.Split(@"\").ToList();
            pathElements = pathElements.Where(o => !string.IsNullOrEmpty(o)).ToList();
            path = HostingEnvironment.WebRootPath;
            foreach (var pathElement in pathElements)
            {
                path += @"\" + pathElement;
            }

            if (path.Contains(HostingEnvironment.WebRootPath))
            {
                while (path.Contains(HostingEnvironment.WebRootPath))
                {
                    path = path.ReplaceFirst(HostingEnvironment.WebRootPath, "");
                }

                path = HostingEnvironment.WebRootPath + path;
            }
            
            return Path.GetFullPath(path);
        }

        protected FileManagerEntry VirtualizePath(FileManagerEntry entry)
        {
            entry.Path = entry.Path.Replace(Path.Combine(HostingEnvironment.WebRootPath), "").Replace(@"\", "/");
            if (entry.Path[0] == '/')
                entry.Path = entry.Path.Remove(0, 1);
            return entry;
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

        protected FileManagerEntry CopyEntry(string target, FileManagerEntry entry)
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

        protected void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
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

        protected FileManagerEntry CreateNewFolder(string target, FileManagerEntry entry)
        {
            FileManagerEntry newEntry;
            var path = NormalizePath(target);
            var physicalPath = EnsureUniqueName(path, entry);

            Directory.CreateDirectory(physicalPath);

            newEntry = _directoryBrowser.GetDirectory(physicalPath);

            return newEntry;
        }

        protected string EnsureUniqueName(string target, FileManagerEntry entry)
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

        protected void DeleteFile(string path)
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

        protected void DeleteDirectory(string path)
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
        
        public JsonResult Read(string target, string filter)
        {
            var path = NormalizePath(target);
            
            if (Authorize(path))
            {
                try
                {
                    var files = _directoryBrowser.GetFiles(path, !string.IsNullOrEmpty(filter)?filter:Filter);
                    var directories = _directoryBrowser.GetDirectories(path);
                    var result = files.Concat(directories).Select(VirtualizePath);

                    return Json(result.ToArray());
                }
                catch (DirectoryNotFoundException)
                {
                    throw new Exception("File Not Found");
                }
            }

            throw new Exception("Forbidden");
        }

        /// <summary>
        /// Updates an entry with a given entry.
        /// </summary>
        /// <param name="path">The path to the parent folder in which the folder should be created.</param>
        /// <param name="entry">The entry.</param>
        /// <returns>An empty <see cref="ContentResult"/>.</returns>
        /// <exception cref="HttpException">Forbidden</exception>
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

        protected FileManagerEntry RenameEntry(FileManagerEntry entry)
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

        /// <summary>
        /// Determines if a file can be uploaded to a given path.
        /// </summary>
        /// <param name="path">The path to which the file should be uploaded.</param>
        /// <param name="file">The file which should be uploaded.</param>
        /// <returns>true if the upload is allowed, otherwise false.</returns>
        public bool AuthorizeUpload(string path, IFormFile file)
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

        private bool IsValidFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var allowedExtensions = Filter.Split(',');

            return allowedExtensions.Any(e => e.Equals("*.*") || e.EndsWith(extension, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Uploads a file to a given path.
        /// </summary>
        /// <param name="path">The path to which the file should be uploaded.</param>
        /// <param name="file">The file which should be uploaded.</param>
        /// <returns>A <see cref="JsonResult"/> containing the uploaded file's size and name.</returns>
        /// <exception cref="HttpException">Forbidden</exception>
        [AcceptVerbs("POST")]
        [DisableValidation]
        [IgnoreAntiforgeryToken]
        public virtual ActionResult Upload(string path, IFormFile file)
        {
            FileManagerEntry newEntry;
            path = NormalizePath(path);
            var fileName = Path.GetFileName(file.FileName);

            if (AuthorizeUpload(path, file))
            {
                SaveFile(file, path);
                newEntry = _directoryBrowser.GetFile(Path.Combine(path, fileName));

                return Json(VirtualizePath(newEntry));
            }

            throw new Exception("Forbidden");
        }

        protected void SaveFile(IFormFile file, string pathToSave)
        {
            try
            {
                var path = Path.Combine(pathToSave, GetFileName(file));
                using var stream = System.IO.File.Create(path);
                file.CopyTo(stream);
            }
            catch (Exception ex)
            {
                Logger.Error("Upload - SaveFile",ex);
                throw new Exception(ex.Message);
            }
        }

        public string GetFileName(IFormFile file)
        {
            var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            return Path.GetFileName(fileContent.FileName.Trim('"'));
        }
    }
    
    public interface IContentProviderController
    {
        JsonResult Read(string target, string filter);

        ActionResult Destroy(FileManagerEntry entry);

        ActionResult Create(string target, FileManagerEntry entry);

        ActionResult Update(string target, FileManagerEntry entry);

        ActionResult Upload(string path, IFormFile file);
    }
}
