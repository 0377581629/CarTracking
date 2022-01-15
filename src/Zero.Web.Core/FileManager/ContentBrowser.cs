using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Zero.Web.FileManager.Interfaces;
using Zero.Web.FileManager.Model;

namespace Zero.Web.FileManager
{
    public class ContentBrowser : IContentBrowser
    {
        public virtual IWebHostEnvironment HostingEnvironment { get; set; }

        public IEnumerable<FileManagerViewModel> GetFiles(
            string path,
            string filter)
        {
            var directoryInfo = new DirectoryInfo(path);
            return (filter ?? "*").Split(new[]
            {
                ", ",
                ",",
                "; ",
                ";"
            }, StringSplitOptions.RemoveEmptyEntries).SelectMany(directoryInfo.GetFiles).Select(
                (Func<FileInfo, FileManagerViewModel>)(file => new FileManagerViewModel
                {
                    Name = Path.GetFileNameWithoutExtension(file.Name),
                    Size = file.Length,
                    Path = file.FullName,
                    ActualPath = file.FullName,
                    Extension = file.Extension,
                    IsDirectory = false,
                    HasDirectories = false,
                    Modified = file.LastWriteTime,
                    ModifiedUtc = file.LastWriteTimeUtc
                }));
        }

        public IEnumerable<FileManagerViewModel> GetDirectories(string path) => new DirectoryInfo(path).GetDirectories().Select(
            (Func<DirectoryInfo, FileManagerViewModel>)(subDirectory => new FileManagerViewModel
            {
                Name = subDirectory.Name,
                Path = subDirectory.FullName,
                ActualPath = subDirectory.FullName,
                Extension = subDirectory.Extension,
                IsDirectory = true,
                HasDirectories = (uint)subDirectory.GetDirectories().Length > 0U,
                Modified = subDirectory.LastWriteTime,
                ModifiedUtc = subDirectory.LastWriteTimeUtc
            }));

        public FileManagerViewModel GetDirectory(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            return new FileManagerViewModel
            {
                Name = directoryInfo.Name,
                Path = directoryInfo.FullName,
                ActualPath = directoryInfo.FullName,
                Extension = directoryInfo.Extension,
                IsDirectory = true,
                HasDirectories = (uint)directoryInfo.GetDirectories().Length > 0U,
                Modified = directoryInfo.LastWriteTime,
                ModifiedUtc = directoryInfo.LastWriteTimeUtc
            };
        }

        public FileManagerViewModel GetFile(string path)
        {
            var fileInfo = new FileInfo(path);
            return new FileManagerViewModel
            {
                Name = Path.GetFileNameWithoutExtension(fileInfo.Name),
                Path = fileInfo.FullName,
                ActualPath = fileInfo.FullName,
                Size = fileInfo.Length,
                Extension = fileInfo.Extension,
                IsDirectory = false,
                HasDirectories = false,
                Modified = fileInfo.LastWriteTime,
                ModifiedUtc = fileInfo.LastWriteTimeUtc
            };
        }
    }
}