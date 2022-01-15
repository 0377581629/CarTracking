using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Zero.Web.FileManager.Model;

namespace Zero.Web.FileManager.Interfaces
{
    public interface IContentBrowser
    {
        IEnumerable<FileManagerViewModel> GetFiles(string path, string filter);

        IEnumerable<FileManagerViewModel> GetDirectories(string path);

        FileManagerViewModel GetDirectory(string path);

        FileManagerViewModel GetFile(string path);

        IWebHostEnvironment HostingEnvironment { get; set; }
    }
}